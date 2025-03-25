using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ARSounds.Application.ImageRecognition;
using ARSounds.Core.Targets;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.FontIcons;
using ARSounds.UI.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.Audio;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using OpenVision.Maui.Controls;
using OpenVision.Core.Reco;

namespace ARSounds.UI.Camera.ViewModels;

public partial class CameraViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly string _apiKey = "Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=";
    private readonly ITargetsService _targetsService;
    private readonly INavigationService _navigationService;
    private readonly IAudioManager _audioManager;
    private readonly CloudRecognition _cloudRecognition;

    private Target? _target;
    private string? _lastTargetId;
    private IAudioPlayer? _audioPlayer;
    private IEnumerable<Target> _targets;
    private CameraButton _currentButton;
    private PolygonImageView _image;

    [ObservableProperty]
    private int _position = 1;

    #endregion

    #region Properties

    public ObservableCollection<CameraButton> Buttons { get; } = new ObservableCollection<CameraButton>();

    #endregion

    public CameraViewModel(IAudioManager audioManager,
                           ITargetsService targetsService,
                           INavigationService navigationService) : base(navigationService)
    {
        _audioManager = audioManager;
        _targetsService = targetsService;
        _navigationService = navigationService;
        _cloudRecognition = new CloudRecognition();
    }

    #region Relay Commands

    [RelayCommand]
    private void CurrentItemChanged(CameraButton cameraButton)
    {
        if (_currentButton is not null)
        {
            _currentButton.IsCurrent = false;
        }

        if (cameraButton is not null)
        {
            cameraButton.IsCurrent = true;
        }

        _currentButton = cameraButton;
    }

    [RelayCommand]
    private void CameraLoaded(ARCamera cameraView)
    {
        cameraView.SetRecoService(_cloudRecognition);
    }

    [RelayCommand]
    private void ImageLoaded(PolygonImageView image)
    {
        _image = image;
    }

    [RelayCommand]
    private void Prev()
    {
        if (Position < 1) return;

        Position--;
    }

    [RelayCommand]
    private void Next()
    {
        if (Position > 1) return;

        Position++;
    }

    [RelayCommand]
    private void SwitchCameraFlash()
    {
    }

    [RelayCommand]
    private async Task OpenCameraSettings()
    {
        await _navigationService.PushAsync(typeof(SettingsViewModel));
    }

    [RelayCommand]
    private async Task CloseCamera()
    {
        await _navigationService.PopModalAsync();
    }

    [RelayCommand]
    private void TrackFound(TargetMatchingResult targetMatchingResult)
    {
        try
        {
            Debug.WriteLine($"id: {targetMatchingResult.Id}");
            Debug.WriteLine($"CenterX: {targetMatchingResult.CenterX}");
            Debug.WriteLine($"CenterY: {targetMatchingResult.CenterY}");
            Debug.WriteLine($"Angle: {targetMatchingResult.Angle}");
            Debug.WriteLine($"Projected region: {string.Join(", ", targetMatchingResult.ProjectedRegion.Select(point => $"X: {point.X}, Y: {point.Y}"))}");
            Debug.WriteLine($"Size: Width: {targetMatchingResult.Size.Width}, Height: {targetMatchingResult.Size.Height}");

            if (!targetMatchingResult.Id.Equals(_lastTargetId))
            {
                _lastTargetId = targetMatchingResult.Id;
                _target = _targets.FirstOrDefault(x => x.VisionTargetId?.ToString() == targetMatchingResult.Id);

                var audioBase64 = Regex.Replace(_target.AudioBase64, "^data:audio/[^;]+;base64,", "");
                var audioBytes = Convert.FromBase64String(audioBase64);

                _audioPlayer?.Stop();
                _audioPlayer?.Dispose();

                _audioPlayer = _audioManager.CreatePlayer(new MemoryStream(audioBytes));

                var imageBase64 = Regex.Replace(_target.ImageBase64, "^data:image/[^;]+;base64,", "");
                var imageBytes = Convert.FromBase64String(imageBase64);

                _image.Buffer = imageBytes;

                _audioPlayer?.Play();
            }

            _image.Points = targetMatchingResult.ProjectedRegion;
            _image.InvalidateSurface();

            var projectedRegion = targetMatchingResult.ProjectedRegion;
            var size = targetMatchingResult.Size;
            var centerX = targetMatchingResult.CenterX;
            var centerY = targetMatchingResult.CenterY;
            var angle = targetMatchingResult.Angle;

        }
        catch (Exception)
        {
        }
    }

    [RelayCommand]
    private void TrackLost()
    {
        try
        {
            _target = null;
            _lastTargetId = null;

            _image.Buffer = null;

            if (_audioPlayer.IsPlaying)
            {
                _audioPlayer.Stop();
            }

            _audioPlayer.Dispose();
            _audioPlayer = null;
        }
        catch (Exception)
        {
        }
    }

    #endregion

    #region Methods

    public override async Task InitializeAsync(object initParams)
    {
        var responseMessage = await _targetsService.GetAsync();

        _targets = responseMessage.Response.Result;

        await _cloudRecognition.InitAsync(_apiKey);

        await Task.Run(() =>
        {
            Buttons.Add(new CameraButton() { Icon = IonIcons.Camera, Text = "Camera" });
            Buttons.Add(new CameraButton() { Icon = IonIcons.Videocamera, Text = "Video" });
            Buttons.Add(new CameraButton() { Icon = IonIcons.IosBarcode, Text = "Barcode" });
        });
    }

    #endregion
}

public class PolygonImageView : SKCanvasView
{
    public System.Drawing.PointF[] Points { get; set; }

    public byte[]? Buffer { get; set; }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        if (Buffer is null) return;

        SKBitmap bitmap;
        using (var stream = new MemoryStream(Buffer))
        {
            bitmap = SKBitmap.Decode(stream);
        }

        SK3dView view = new SK3dView();

        SKPath path = new SKPath();

        path.AddPoly(Points.Select(x => new SKPoint(x.X, x.Y)).ToArray());

        canvas.ClipPath(path, SKClipOperation.Intersect, true);
        canvas.DrawBitmap(bitmap, new SKRect(0, 0, bitmap.Width, bitmap.Height));

        canvas.Dispose();
        bitmap.Dispose();
    }
}
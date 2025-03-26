using ARSounds.Application.ImageRecognition;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenVision.WinUI.Controls;

namespace ARSounds.UI.WinUI.Camera.ViewModels;

public partial class ARCameraViewModel : ObservableObject
{
    #region Fields/Consts

    //private readonly string _apiKey = "Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=";
    //private readonly ITargetsService _targetsService;
    //private readonly CloudRecognition _cloudRecognition;

    //private Target? _target;
    //private string? _lastTargetId;
    //private IAudioPlayer? _audioPlayer;
    //private IEnumerable<Target> _targets;

    #endregion

    public ARCameraViewModel(
        //IAudioManager audioManager,
        ITargetsService targetsService)
    {
        //_audioManager = audioManager;
        //_targetsService = targetsService;
        //_cloudRecognition = new CloudRecognition();
    }

    #region Relay Commands

    [RelayCommand]
    private async Task CameraLoaded(ARCamera cameraView)
    {
        //var responseMessage = await _targetsService.GetAsync();

        //_targets = responseMessage.Response.Result;

        //await _cloudRecognition.InitAsync(_apiKey);

        //cameraView.SetRecoService(_cloudRecognition);
    }

    [RelayCommand]
    private void SwitchCameraFlash()
    {
    }

    [RelayCommand]
    private void OpenCameraSettings()
    {
    }

    [RelayCommand]
    private void CloseCamera()
    {
    }

    [RelayCommand]
    private void TrackFound(TargetMatchingResult targetMatchingResult)
    {
        //try
        //{
        //    Debug.WriteLine($"id: {targetMatchingResult.Id}");
        //    Debug.WriteLine($"CenterX: {targetMatchingResult.CenterX}");
        //    Debug.WriteLine($"CenterY: {targetMatchingResult.CenterY}");
        //    Debug.WriteLine($"Angle: {targetMatchingResult.Angle}");
        //    Debug.WriteLine($"Projected region: {string.Join(", ", targetMatchingResult.ProjectedRegion.Select(point => $"X: {point.X}, Y: {point.Y}"))}");
        //    Debug.WriteLine($"Size: Width: {targetMatchingResult.Size.Width}, Height: {targetMatchingResult.Size.Height}");

        //    if (!targetMatchingResult.Id.Equals(_lastTargetId))
        //    {
        //        _lastTargetId = targetMatchingResult.Id;
        //        _target = _targets.FirstOrDefault(x => x.VisionTargetId?.ToString() == targetMatchingResult.Id);

        //        var audioBase64 = Regex.Replace(_target.AudioBase64, "^data:audio/[^;]+;base64,", "");
        //        var audioBytes = Convert.FromBase64String(audioBase64);

        //        _audioPlayer?.Stop();
        //        _audioPlayer?.Dispose();

        //        _audioPlayer = _audioManager.CreatePlayer(new MemoryStream(audioBytes));

        //        var imageBase64 = Regex.Replace(_target.ImageBase64, "^data:image/[^;]+;base64,", "");
        //        var imageBytes = Convert.FromBase64String(imageBase64);

        //        _image.Buffer = imageBytes;

        //        _audioPlayer?.Play();
        //    }

        //    //_image.Points = targetMatchingResult.ProjectedRegion;
        //    //_image.InvalidateSurface();

        //    var projectedRegion = targetMatchingResult.ProjectedRegion;
        //    var size = targetMatchingResult.Size;
        //    var centerX = targetMatchingResult.CenterX;
        //    var centerY = targetMatchingResult.CenterY;
        //    var angle = targetMatchingResult.Angle;

        //}
        //catch (Exception)
        //{
        //}
    }

    [RelayCommand]
    private void TrackLost()
    {
        //try
        //{
        //    _target = null;
        //    _lastTargetId = null;

        //    _image.Buffer = null;

        //    if (_audioPlayer.IsPlaying)
        //    {
        //        _audioPlayer.Stop();
        //    }

        //    _audioPlayer.Dispose();
        //    _audioPlayer = null;
        //}
        //catch (Exception)
        //{
        //}
    }

    #endregion

    #region Methods

    #endregion
}
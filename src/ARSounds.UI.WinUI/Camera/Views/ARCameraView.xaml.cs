using ARSounds.Application.Auth;
using ARSounds.Application.ImageRecognition;
using ARSounds.Core.Targets;
using CommonServiceLocator;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NAudio.Wave;
using OpenVision.Core.DataTypes;
using OpenVision.Core.Reco;
using OpenVision.WinUI.Controls;
using System.Text.RegularExpressions;

namespace ARSounds.UI.WinUI.Camera.Views;

public partial class ARCameraView : UserControl
{
    private readonly string _apiKey = "Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=";
    private readonly IAuthService _authService;
    private readonly ITargetsService _targetsService;

    private Target? _target;
    private string? _lastTargetId;
    private IEnumerable<Target>? _targets;

    private WaveOutEvent? _waveOut;
    private WaveStream? _waveStream;
    private Image<Bgra, byte>? _waveformImage;

    public ARCameraView()
    {
        _authService = ServiceLocator.Current.GetInstance<IAuthService>();
        _targetsService = ServiceLocator.Current.GetInstance<ITargetsService>();

        InitializeComponent();
    }

    private async void Camera_Loaded(object sender, RoutedEventArgs e)
    {
        await InitializeAsync();
    }

    private async void Reload_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        await InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        try
        {
            await _authService.TryLoginFromCacheAsync(CancellationToken.None).ConfigureAwait(false);

            var responseMessage = await _targetsService.GetAsync();

            _targets = responseMessage.Response.Result;
        }
        catch
        {
            await _authService.LoginAsync(CancellationToken.None).ConfigureAwait(false);

            var responseMessage = await _targetsService.GetAsync();

            _targets = responseMessage.Response.Result;
        }

        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync(_apiKey);

        Camera.SetRecoService(cloudRecognition);

        System.Windows.Forms.MessageBox.Show("Application Loaded!");
    }

    private void Camera_TrackFound(object sender, TargetMatchingEventArgs e)
    {
        if (e.TargetMatchResults == null || e.TargetMatchResults.Count == 0)
        {
            return;
        }

        var targetMatchResult = e.TargetMatchResults.First();

        _target = _targets?.FirstOrDefault(x => x.VisionTargetId?.ToString() == targetMatchResult.Id);

        if (_target == null)
        {
            return;
        }

        if (!targetMatchResult.Id.Equals(_lastTargetId))
        {
            TargetIdElement.Text = targetMatchResult.Id;
            _lastTargetId = targetMatchResult.Id;

            var audioBase64 = Regex.Replace(_target.AudioBase64, "^data:audio/[^;]+;base64,", "");
            var audioBytes = Convert.FromBase64String(audioBase64);
            PlayAudio(audioBytes);

            _waveformImage = ARCameraHelper.DecodeBase64(_target.ImageBase64);
        }

        // Compute audio playback progress (0.0 to 1.0)
        var audioProgress = 0d;
        if (_waveStream != null && _waveStream.TotalTime.TotalMilliseconds > 0)
        {
            audioProgress = _waveStream.CurrentTime.TotalMilliseconds / _waveStream.TotalTime.TotalMilliseconds;
            audioProgress = Math.Max(0, Math.Min(audioProgress, 1)); // Clamp between 0 and 1
        }

        ARCameraHelper.UpdateOverlayImage(
            _waveformImage!,
            e.Frame,
            targetMatchResult,
            audioProgress,
            _target.HexColor);
    }

    private void Camera_TrackLost(object sender, EventArgs e)
    {
        _waveformImage = null;
        TargetIdElement.Text = null;

        _target = null;
        _lastTargetId = null;

        StopAudio();
    }

    private void PlayAudio(byte[] audioBytes)
    {
        StopAudio();

        _waveStream = new Mp3FileReader(new MemoryStream(audioBytes));
        _waveOut = new WaveOutEvent();
        _waveOut.Init(_waveStream);
        _waveOut.Play();
    }

    private void StopAudio()
    {
        _waveOut?.Stop();
        _waveOut?.Dispose();
        _waveStream?.Dispose();

        _waveOut = null;
        _waveStream = null;
    }
}
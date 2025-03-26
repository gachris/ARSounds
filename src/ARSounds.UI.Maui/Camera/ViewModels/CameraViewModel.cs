using ARSounds.Application.ImageRecognition;
using ARSounds.Core.Targets;
using ARSounds.UI.Maui.Common.ViewModels;
using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
#if WINDOWS
using Emgu.CV;
#else
using OpenCV.Core;
#endif
using OpenVision.Core.Reco;
using OpenVision.Maui.Controls;
using Plugin.Maui.Audio;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ARSounds.UI.Maui.Camera.ViewModels;

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
    private IEnumerable<Target>? _targets;

    private Mat? _waveformImage;

    [ObservableProperty]
    private string? _targetIdElementText;

    #endregion

    #region Properties

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
    private void CameraLoaded(ARCamera cameraView)
    {
        cameraView.SetRecoService(_cloudRecognition);
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
    private void TrackFound(TargetMatchingEventArgs e)
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
            TargetIdElementText = targetMatchResult.Id;
            _lastTargetId = targetMatchResult.Id;

            var audioBase64 = Regex.Replace(_target.AudioBase64, "^data:audio/[^;]+;base64,", "");
            var audioBytes = Convert.FromBase64String(audioBase64);
            PlayAudio(audioBytes);

            _waveformImage = ARCameraHelper.DecodeBase64(_target.ImageBase64);
        }

        // Compute audio playback progress (0.0 to 1.0)
        var audioProgress = 0d;
        if (_audioPlayer != null && _audioPlayer.Duration > 0)
        {
            audioProgress = _audioPlayer.CurrentPosition;
            audioProgress = Math.Max(0, Math.Min(audioProgress, 1)); // Clamp between 0 and 1
        }

        ARCameraHelper.UpdateOverlayImage(
            _waveformImage!,
            e.Frame,
            targetMatchResult,
            audioProgress,
            _target.HexColor);
    }

    [RelayCommand]
    private void TrackLost()
    {
        _waveformImage = null;
        TargetIdElementText = null;

        _target = null;
        _lastTargetId = null;

        StopAudio();
    }

    #endregion

    #region Methods

    public override async Task InitializeAsync(object initParams)
    {
        var responseMessage = await _targetsService.GetAsync();

        _targets = responseMessage.Response.Result;

        await _cloudRecognition.InitAsync(_apiKey);
    }

    private void PlayAudio(byte[] audioBytes)
    {
        StopAudio();

        _audioPlayer = _audioManager.CreatePlayer(new MemoryStream(audioBytes));
        _audioPlayer.Play();
    }

    private void StopAudio()
    {
        if (_audioPlayer is { IsPlaying: true })
        {
            _audioPlayer.Stop();
            _audioPlayer.Dispose();
        }

        _audioPlayer = null;
    }

    #endregion
}
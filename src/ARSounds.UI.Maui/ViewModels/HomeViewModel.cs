using ARSounds.ApplicationFlow;
using ARSounds.Core.Targets;
using ARSounds.Core.Targets.Events;
using ARSounds.UI.Maui.Services;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using OpenVision.Core.Reco;
using Plugin.Maui.Audio;
using OpenVision.Maui.Controls;
using System.Text.RegularExpressions;
using ARSounds.Application.Queries;

#if WINDOWS
using Emgu.CV;
#else
using OpenCV.Core;
#endif

namespace ARSounds.UI.Maui.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    #region Fields/Consts

    private readonly string _apiKey = "Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=";
    private readonly IMediator _mediator;
    private readonly INavigationService _navigationService;
    private readonly IAudioManager _audioManager;
    private readonly CloudRecognition _cloudRecognition;

    private Target? _target;
    private string? _lastTargetId;
    private IAudioPlayer? _audioPlayer;
    private IEnumerable<Target>? _targets;

    private Mat? _waveformImage;

    #endregion

    #region Properties

    #endregion

    public HomeViewModel(
        IMediator mediator,
        IApplicationEvents applicationEvents,
        IAudioManager audioManager,
        INavigationService navigationService) : base(navigationService)
    {
        _mediator = mediator;
        _navigationService = navigationService;
        _audioManager = audioManager;
        _navigationService = navigationService;
        _cloudRecognition = new CloudRecognition();

        applicationEvents.Register<TargetsUpdatedStartedEvent>(OnTargetsUpdatedStarted);
        applicationEvents.Register<TargetsUpdatedFinishedEvent>(OnTargetsUpdatedFinished);
        applicationEvents.Register<TargetsUpdatedEvent>(OnTargetsUpdated);
    }

    #region Methods

    private void OnTargetsUpdatedStarted(TargetsUpdatedStartedEvent obj)
    {
        SetDataLoadingIndicators(true);
        LoadingText = "Loading...";
    }

    private void OnTargetsUpdatedFinished(TargetsUpdatedFinishedEvent obj)
    {
        SetDataLoadingIndicators(false);
    }

    private void OnTargetsUpdated(TargetsUpdatedEvent obj)
    {
        _targets = obj.Items;
    }

    public override async Task InitializeAsync(object? navigationData)
    {
        await _mediator.Send(new GetTargetsQuery());
    }

    #endregion

    #region Relay Commands

    [RelayCommand]
    private async Task OpenUserProfile()
    {
        await _navigationService.PushAsync(typeof(ProfileViewModel));
    }

    [RelayCommand]
    private async Task CameraLoaded(ARCamera cameraView)
    {
        await _cloudRecognition.InitAsync(_apiKey);
        cameraView.SetRecoService(_cloudRecognition);
    }

    [RelayCommand]
    private async Task OpenCameraSettings()
    {
        await _navigationService.PushAsync(typeof(SettingsViewModel));
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

        _target = null;
        _lastTargetId = null;

        StopAudio();
    }

    #endregion

    #region Methods

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

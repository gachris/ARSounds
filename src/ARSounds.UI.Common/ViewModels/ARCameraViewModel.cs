using ARSounds.ApplicationFlow;
using ARSounds.Core.Targets;
using ARSounds.UI.Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ARSounds.Core.Targets.Events;
using ARSounds.Application.Queries;
using MediatR;
using System.Text.RegularExpressions;
using ARSounds.UI.Common.Camera;
using ARSounds.UI.Common.Data;
using ARSounds.Core.ClaimsPrincipal.Events;
using ARSounds.Application.Configuration;
#if WINDOWS
using Emgu.CV;
using Emgu.CV.Structure;
#else
using OpenCV.Core;
#endif
using NAudio.Wave;

namespace ARSounds.UI.Common.ViewModels;

public partial class ARCameraViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    private readonly IMediator _mediator;

    private WaveOutEvent? _waveOut;

    private bool _isBusy;

    #endregion

    #region Properties

    public string ClientApiKey { get; }

    protected WaveStream? WaveStream { get; private set; }

    protected IEnumerable<Target>? Targets { get; private set; }

    protected string? LastTargetId { get; set; }

    protected Target? Target { get; set; }

#if WINDOWS
    protected Image<Bgra, byte>? WaveformImage { get; set; }
#else
    protected Mat? WaveformImage { get; set; }
#endif

    public bool IsBusy
    {
        get => _isBusy;
        protected set => SetProperty(ref _isBusy, value);
    }

    #endregion

    public ARCameraViewModel(
        IMediator mediator,
        IApplicationEvents applicationEvents,
        AppConfiguration appConfiguration)
    {
        _mediator = mediator;
        ClientApiKey = appConfiguration.OpenVisionClientApiKey;

        applicationEvents.Register<SignedInEvent>(OnSignedIn);
        applicationEvents.Register<RetrieveTargetsStartedEvent>(OnTargetsUpdatedStarted);
        applicationEvents.Register<RetrieveTargetsFinishedEvent>(OnTargetsUpdatedFinished);
        applicationEvents.Register<TargetsCollectionUpdatedEvent>(OnTargetsUpdated);
    }

    #region Relay Commands

    [RelayCommand]
    private void TrackFound(TargetMatchingResult e)
    {
        if (e.TargetMatchResults == null || e.TargetMatchResults.Count == 0)
        {
            return;
        }

        var targetMatchResult = e.TargetMatchResults.First();

        Target = Targets?.FirstOrDefault(x => x.VisionTargetId?.ToString() == targetMatchResult.Id);

        if (Target == null)
        {
            return;
        }

        if (!targetMatchResult.Id.Equals(LastTargetId))
        {
            LastTargetId = targetMatchResult.Id;

            var audioBase64 = Regex.Replace(Target.AudioBase64, "^data:audio/[^;]+;base64,", "");
            var audioBytes = Convert.FromBase64String(audioBase64);
            PlayAudio(audioBytes);

            WaveformImage = ARCameraHelper.DecodeBase64(Target.ImageBase64!);
        }

        var audioProgress = 0d;
        if (WaveStream != null && WaveStream.TotalTime.TotalMilliseconds > 0)
        {
            audioProgress = WaveStream.CurrentTime.TotalMilliseconds / WaveStream.TotalTime.TotalMilliseconds;
            audioProgress = Math.Max(0, Math.Min(audioProgress, 1));
        }

        ARCameraHelper.UpdateOverlayImage(
            WaveformImage!,
            e.Frame,
            targetMatchResult,
            audioProgress,
            Target.HexColor!);
    }

    [RelayCommand]
    private void TrackLost()
    {
        Target = null;
        LastTargetId = null;
        WaveformImage = null;

        StopAudio();
    }

    #endregion

    #region Methods

    public virtual async void OnNavigated()
    {
        await _mediator.Send(new RetrieveTargetsQuery());
    }

    public virtual void OnNavigatedAway()
    {
    }

    protected virtual void PlayAudio(byte[] audioBytes)
    {
        StopAudio();

        WaveStream = new Mp3FileReader(new MemoryStream(audioBytes));
        _waveOut = new WaveOutEvent();
        _waveOut.Init(WaveStream);
        _waveOut.Play();
    }

    protected virtual void StopAudio()
    {
        _waveOut?.Stop();
        _waveOut?.Dispose();
        WaveStream?.Dispose();

        _waveOut = null;
        WaveStream = null;
    }

    #endregion

    #region Events Subscriptions

    private void OnTargetsUpdatedStarted(RetrieveTargetsStartedEvent obj)
    {
        IsBusy = true;
    }

    private void OnTargetsUpdatedFinished(RetrieveTargetsFinishedEvent obj)
    {
        IsBusy = false;
    }

    private void OnTargetsUpdated(TargetsCollectionUpdatedEvent obj)
    {
        Targets = obj.Items;
    }

    private async void OnSignedIn(SignedInEvent obj)
    {
        await _mediator.Send(new RetrieveTargetsQuery());
    }

    #endregion
}
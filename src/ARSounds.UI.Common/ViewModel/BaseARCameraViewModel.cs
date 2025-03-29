using ARSounds.Application.Services;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using ARSounds.Core.Targets;
using ARSounds.UI.Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using Emgu.CV.Structure;
using NAudio.Wave;

namespace ARSounds.UI.Common.ViewModels;

public abstract partial class BaseARCameraViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    protected const string ApiKey = "Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=";

    private readonly ITargetsService _targetsService;

    private WaveOutEvent? _waveOut;

    #endregion

    #region Properties

    protected WaveStream? WaveStream { get; private set; }

    protected IEnumerable<Target>? Targets { get; private set; }

    protected string? LastTargetId { get; set; }

    protected Target? Target { get; set; }

    protected Image<Bgra, byte>? WaveformImage { get; set; }

    #endregion

    public BaseARCameraViewModel(
        ITargetsService targetsService,
        IApplicationEvents applicationEvents)
    {
        _targetsService = targetsService;

        applicationEvents.Register<SignedInEvent>(OnSignedIn);
    }

    #region Relay Commands

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

    public virtual void OnNavigated(object? parameter)
    {
    }

    public virtual void OnNavigatedAway()
    {
    }

    protected void PlayAudio(byte[] audioBytes)
    {
        StopAudio();

        WaveStream = new Mp3FileReader(new MemoryStream(audioBytes));
        _waveOut = new WaveOutEvent();
        _waveOut.Init(WaveStream);
        _waveOut.Play();
    }

    protected void StopAudio()
    {
        _waveOut?.Stop();
        _waveOut?.Dispose();
        WaveStream?.Dispose();

        _waveOut = null;
        WaveStream = null;
    }

    #endregion

    private async void OnSignedIn(SignedInEvent obj)
    {
        var responseMessage = await _targetsService.GetAsync();
        Targets = responseMessage.Response.Result;
    }
}
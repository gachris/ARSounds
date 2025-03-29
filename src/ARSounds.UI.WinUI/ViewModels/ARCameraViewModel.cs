using System.Text.RegularExpressions;
using ARSounds.Application.Services;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Auth.Events;
using ARSounds.Core.Targets;
using ARSounds.UI.Common.Camera;
using ARSounds.UI.Common.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Emgu.CV;
using Emgu.CV.Structure;
using NAudio.Wave;
using OpenVision.Core.Reco;
using OpenVision.WinUI.Controls;

namespace ARSounds.UI.WinUI.ViewModels;

public partial class ARCameraViewModel : ObservableObject, IViewModelAware
{
    #region Fields/Consts

    private readonly string _apiKey = "Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=";
    private readonly ITargetsService _targetsService;

    private Target? _target;
    private string? _lastTargetId;
    private IEnumerable<Target>? _targets;

    private WaveOutEvent? _waveOut;
    private WaveStream? _waveStream;
    private Image<Bgra, byte>? _waveformImage;

    #endregion

    public ARCameraViewModel(
        ITargetsService targetsService,
        IApplicationEvents applicationEvents)
    {
        _targetsService = targetsService;

        applicationEvents.Register<SignedInEvent>(OnSignedIn);
    }

    #region Relay Commands

    [RelayCommand]
    private async Task CameraLoaded(ARCamera camera)
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync(_apiKey);

        camera.SetRecoService(cloudRecognition);
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

            _waveformImage = ARCameraHelper.DecodeBase64(_target.ImageBase64!);
        }

        var audioProgress = 0d;
        if (_waveStream != null && _waveStream.TotalTime.TotalMilliseconds > 0)
        {
            audioProgress = _waveStream.CurrentTime.TotalMilliseconds / _waveStream.TotalTime.TotalMilliseconds;
            audioProgress = Math.Max(0, Math.Min(audioProgress, 1));
        }

        ARCameraHelper.UpdateOverlayImage(
            _waveformImage!,
            e.Frame,
            targetMatchResult,
            audioProgress,
            _target.HexColor!);
    }

    [RelayCommand]
    private void TrackLost()
    {
        _target = null;
        _lastTargetId = null;
        _waveformImage = null;

        StopAudio();
    }

    #endregion

    #region Methods

    public void OnNavigated(object? parameter)
    {
    }

    public void OnNavigatedAway()
    {
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

    #endregion

    private async void OnSignedIn(SignedInEvent obj)
    {
        var responseMessage = await _targetsService.GetAsync();
        _targets = responseMessage.Response.Result;
    }
}
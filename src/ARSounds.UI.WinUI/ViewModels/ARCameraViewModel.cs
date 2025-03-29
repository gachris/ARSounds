using System.Text.RegularExpressions;
using ARSounds.Application.Services;
using ARSounds.ApplicationFlow;
using ARSounds.Core.Targets;
using ARSounds.UI.Common.Camera;
using ARSounds.UI.Common.ViewModels;
using CommunityToolkit.Mvvm.Input;
using OpenVision.Core.Reco;
using OpenVision.WinUI.Controls;

namespace ARSounds.UI.WinUI.ViewModels;

public partial class ARCameraViewModel : BaseARCameraViewModel
{
    public ARCameraViewModel(
        ITargetsService targetsService,
        IApplicationEvents applicationEvents) : base(targetsService, applicationEvents)
    {
    }

    #region Relay Commands

    [RelayCommand]
    private async Task CameraLoaded(ARCamera camera)
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync(ApiKey);

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

    #endregion
}
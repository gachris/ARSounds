using ARSounds.Application.ImageRecognition;
using ARSounds.Core.Targets;
using CommonServiceLocator;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using NAudio.Wave;
using OpenVision.Core.DataTypes;
using OpenVision.Core.Reco;
using OpenVision.Wpf.Controls;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace ARSounds.UI.Wpf.Camera.Views;

/// <summary>
/// Interaction logic for ARSoundsCamera.xaml
/// </summary>
public partial class ARCameraView : UserControl
{
    private readonly string _apiKey = "Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=";

    private readonly ITargetsService _targetsService;

    private Target? _target;
    private string? _lastTargetId;
    private IEnumerable<Target>? _targets;

    private IWavePlayer? _waveOut;
    private WaveStream? _waveStream;

    public ARCameraView()
    {
        _targetsService = ServiceLocator.Current.GetInstance<ITargetsService>();

        InitializeComponent();
    }

    private async void Camera_Loaded(object sender, RoutedEventArgs e)
    {
        // Create a new ImageRequestBuilder instance with specific configurations
        var newBuilder = new ImageRequestBuilder()
            .WithGrayscale()
            .WithGaussianBlur(new System.Drawing.Size(5, 5), 0)
            .WithLowResolution(320);

        // Get the type of the camera object
        var type = Camera.GetType();

        // Get the FieldInfo for the private readonly field '_imageRequestBuilder'
        var fieldInfo = type.GetField("_imageRequestBuilder", BindingFlags.NonPublic | BindingFlags.Instance);

        // Set the value of the private readonly field using reflection
        fieldInfo?.SetValue(Camera, newBuilder);

        var responseMessage = await _targetsService.GetAsync();

        _targets = responseMessage.Response.Result;

        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync(_apiKey);

        Camera.SetRecoService(cloudRecognition);
    }

    private void Camera_TrackFound(object sender, TargetMatchingEventArgs e)
    {
        try
        {
            var targetMatchResult = e.TargetMatchResults.First();

            TargetIdElement.Text = targetMatchResult.Id;

            if (!targetMatchResult.Id.Equals(_lastTargetId))
            {
                _lastTargetId = targetMatchResult.Id;
                _target = _targets?.FirstOrDefault(x => x.VisionTargetId?.ToString() == targetMatchResult.Id);

                if (_target is null)
                {
                    return;
                }

                var audioBase64 = Regex.Replace(_target.AudioBase64, "^data:audio/[^;]+;base64,", "");
                var audioBytes = Convert.FromBase64String(audioBase64);

                PlayAudio(audioBytes);
            }

            if (_target is null)
            {
                return;
            }

            if (e.Frame.NumberOfChannels != 4)
            {
                CvInvoke.CvtColor(e.Frame, e.Frame, ColorConversion.Bgr2Bgra);
            }

            var base64Image = Regex.Replace(_target.ImageBase64, "^data:image/[^;]+;base64,", "");
            var imageBytes = Convert.FromBase64String(base64Image);

            var imageMat = new Mat();

            // Decode image bytes directly to Image<Bgra, byte> without Bitmap
            CvInvoke.Imdecode(imageBytes, ImreadModes.Unchanged, imageMat);
            Image<Bgra, byte> overlay = imageMat.ToImage<Bgra, byte>();

            // Proceed with your polygon projection
            var polyPoints = Array.ConvertAll(targetMatchResult.ProjectedRegion, System.Drawing.Point.Round);

            var srcCorners = new PointF[]
            {
                new(0, 0),
                new(overlay.Width - 1, 0),
                new(overlay.Width - 1, overlay.Height - 1),
                new(0, overlay.Height - 1)
            };

            var dstCorners = polyPoints.Select(p => new PointF(p.X, p.Y)).Reverse().ToArray();

            var perspectiveMatrix = CvInvoke.GetPerspectiveTransform(srcCorners, dstCorners);

            var warpedOverlay = new Mat(e.Frame.Size, DepthType.Cv8U, 4);
            warpedOverlay.SetTo(new MCvScalar(0, 0, 0, 0));
            CvInvoke.WarpPerspective(overlay, warpedOverlay, perspectiveMatrix, e.Frame.Size, Inter.Linear, Warp.Default, BorderType.Constant);

            var warpedChannels = warpedOverlay.Split();
            var warpedAlpha = warpedChannels[3];

            var maskInv = new Mat();
            CvInvoke.BitwiseNot(warpedAlpha, maskInv);

            var frameBgra = e.Frame.Clone();
            var bg = new Mat();
            CvInvoke.BitwiseAnd(frameBgra, frameBgra, bg, maskInv);

            var fg = new Mat();
            CvInvoke.BitwiseAnd(warpedOverlay, warpedOverlay, fg, warpedAlpha);

            CvInvoke.Add(bg, fg, e.Frame);
        }
        catch
        {
        }
    }

    private void Camera_TrackLost(object sender, EventArgs e)
    {
        try
        {
            TargetIdElement.Text = null;

            _target = null;
            _lastTargetId = null;

            StopAudio();
        }
        catch (Exception)
        {
        }
    }

    private async void Reload_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync(_apiKey);

        Camera.SetRecoService(cloudRecognition);
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

using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.UI.Xaml;
using OpenVision.Core.Reco;
using OpenVision.WinUI.Controls;

namespace ARSounds.WinUI.Host;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void Camera_Loaded(object sender, RoutedEventArgs e)
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync("Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=");

        Camera.SetRecoService(cloudRecognition);
    }

    private void Camera_TrackFound(object sender, TargetMatchingEventArgs e)
    {
        var targetMatchResult = e.TargetMatchResults.First();
        TargetIdElement.Text = targetMatchResult.Id;

        var points = Array.ConvertAll(targetMatchResult.ProjectedRegion, System.Drawing.Point.Round);
        using var vp = new VectorOfPoint(points);
        CvInvoke.Polylines(e.Frame, vp, true, new MCvScalar(255, 0, 0, 255), 5);

        var arrowpts = new[]
        {
            new System.Drawing.PointF(targetMatchResult.CenterX, targetMatchResult.CenterY),
            new System.Drawing.PointF(targetMatchResult.CenterX, targetMatchResult.CenterY + 10),
            new System.Drawing.PointF(targetMatchResult.CenterX, targetMatchResult.CenterY - 10),
            new System.Drawing.PointF(targetMatchResult.CenterX, targetMatchResult.CenterY),
            new System.Drawing.PointF(targetMatchResult.CenterX + 10, targetMatchResult.CenterY),
            new System.Drawing.PointF(targetMatchResult.CenterX - 10, targetMatchResult.CenterY)
        };

        var arrowpoints = Array.ConvertAll(arrowpts, System.Drawing.Point.Round);
        using var vps = new VectorOfPoint(arrowpoints);
        CvInvoke.Polylines(e.Frame, vps, true, new MCvScalar(0, 0, 255, 255), 2);
    }

    private void Camera_TrackLost(object sender, EventArgs e)
    {
        TargetIdElement.Text = null;
    }

    private async void Reload_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync("Dq6moD7K0U7S0JRp570QnZRRWc4nykBBcPIF736ZMWg=");

        Camera.SetRecoService(cloudRecognition);
    }
}

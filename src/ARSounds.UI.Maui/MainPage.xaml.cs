#if ANDROID
using OpenCV.Core;
using OpenCV.ImgProc;
#elif WINDOWS
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
#endif
using OpenVision.Maui.Controls;
using OpenVision.Core.Dataset;
using OpenVision.Core.Reco;

namespace OpenVision.Maui.Demo.ARCamera;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Camera_Loaded(object sender, EventArgs e)
    {
        var recognition = await InitImageRecognition();
        Camera.SetRecoService(recognition);

        Camera.TrackFound += Camera_TrackFound;
    }

    private void Camera_TrackFound(object? sender, TargetMatchingEventArgs e)
    {
#if ANDROID    
        foreach (var targetMatchResult in e.TargetMatchResults)
        {
            var projectedRegion = Array.ConvertAll(targetMatchResult.ProjectedRegion.ToArray(), p => new OpenCV.Core.Point(p.X, p.Y));

            using var vp = new MatOfPoint(projectedRegion);
            Imgproc.Polylines(e.Frame, new List<MatOfPoint> { vp }, true, new Scalar(255, 0, 0, 255), 5);

            var arrowpts = new MatOfPoint2f(
            [
                new OpenCV.Core.Point(targetMatchResult.CenterX, targetMatchResult.CenterY),
            new OpenCV.Core.Point(targetMatchResult.CenterX, targetMatchResult.CenterY + 10),
            new OpenCV.Core.Point(targetMatchResult.CenterX, targetMatchResult.CenterY - 10),
            new OpenCV.Core.Point(targetMatchResult.CenterX, targetMatchResult.CenterY),
            new OpenCV.Core.Point(targetMatchResult.CenterX + 10, targetMatchResult.CenterY),
            new OpenCV.Core.Point(targetMatchResult.CenterX - 10, targetMatchResult.CenterY)
            ]);

            var arrowpoints = arrowpts.ToArray();
            using var vps = new MatOfPoint(arrowpoints);
            Imgproc.Polylines(e.Frame, new List<MatOfPoint> { vps }, true, new Scalar(0, 0, 255, 255), 2);
        }
#elif WINDOWS
        foreach (var targetMatchResult in e.TargetMatchResults)
        {
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
#endif
    }

    private async void Image_Reco_Button_Click(object sender, EventArgs e)
    {
        var recognition = await InitImageRecognition();
        Camera.SetRecoService(recognition);
    }

    private async void Db_Reco_Button_Click(object sender, EventArgs e)
    {
        var recognition = await InitDatasetRecognition();
        Camera.SetRecoService(recognition);
    }

    private async void Cloud_Reco_Button_Click(object sender, EventArgs e)
    {
        var recognition = await InitCloudRecognition();
        Camera.SetRecoService(recognition);
    }

    private static async Task<IRecognition> InitCloudRecognition()
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync("o6a6bA3jFlUTn+YE6LnDsdDHrWWIDH6ppOE76jv84H4=");

        return cloudRecognition;
    }

    private static async Task<IRecognition> InitImageRecognition()
    {
        var imageRecognition = new ImageRecognition();

        using var stream1 = await FileSystem.OpenAppPackageFileAsync("img1.jpg").ConfigureAwait(false);
        using var stream2 = await FileSystem.OpenAppPackageFileAsync("img2.jpg").ConfigureAwait(false);

        var img1 = Core.Reco.DataTypes.ImageData.Load(stream1);
        var img2 = Core.Reco.DataTypes.ImageData.Load(stream2);
        var imgs = new List<Core.Reco.DataTypes.ImageData>() { img1, img2 };

        await imageRecognition.InitAsync(imgs);

        return imageRecognition;
    }

    private static async Task<IRecognition> InitDatasetRecognition()
    {
        var imageRecognition = new ImageRecognition();

        using var databasestream = await FileSystem.OpenAppPackageFileAsync("device-db.bin").ConfigureAwait(false);
        var targetDataset = await TargetDataset.LoadAsync(databasestream);

        await imageRecognition.InitAsync(targetDataset);

        return imageRecognition;
    }
}


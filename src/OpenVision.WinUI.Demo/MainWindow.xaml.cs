using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.UI.Xaml;
using OpenVision.Core.Reco;
using OpenVision.WinUI.Controls;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenVision.WinUI.Demo;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
    }

    private async void ARCamera_Loaded(object sender, RoutedEventArgs e)
    {
        var recognition = await InitImageRecognition();
        Camera.SetRecoService(recognition);
        Camera.TrackFound += Camera_TrackFound;
    }

    private void Camera_TrackFound(object? sender, TargetMatchingEventArgs e)
    {
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
    }

    private static async Task<IRecognition> InitImageRecognition()
    {
        var imageRecognition = new ImageRecognition();

        // Get files from the app's package
        StorageFile file1 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/img1.jpg"));
        StorageFile file2 = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/img2.jpg"));

        // Open the files as streams
        using var stream1 = await file1.OpenStreamForReadAsync();
        using var stream2 = await file2.OpenStreamForReadAsync();

        // Load the images using your Core.Reco.DataTypes.ImageData loader
        var img1 = Core.Reco.DataTypes.ImageData.Load(stream1);
        var img2 = Core.Reco.DataTypes.ImageData.Load(stream2);

        // Prepare list of images
        var imgs = new List<Core.Reco.DataTypes.ImageData>() { img1, img2 };

        // Initialize the image recognition process
        await imageRecognition.InitAsync(imgs);

        return imageRecognition;
    }
}

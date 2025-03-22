using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenVision.Core.Configuration;
using OpenVision.Core.Dataset;
using OpenVision.Core.Reco;
using OpenVision.Core.Reco.DataTypes;
using OpenVision.Wpf.Controls;
using System.Windows;
using System.Windows.Media.Media3D;

namespace Vision.Wpf.Demo.ARScene;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow2 : Window
{
    public MainWindow2()
    {
        VisionSystemConfig.WebSocketUrl = "wss://localhost:44320/ws";
        InitializeComponent();
    }

    private async void Camera_Loaded(object sender, RoutedEventArgs e)
    {
        var recognition = await InitImageRecognition();
        Camera.SetRecoService(recognition);
    }

    private async void Image_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        var recognition = await InitImageRecognition();
        Camera.SetRecoService(recognition);
    }

    private async void Db_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        var recognition = await InitDatasetRecognition();
        Camera.SetRecoService(recognition);
    }

    private async void Cloud_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        var recognition = await InitCloudRecognition();
        Camera.SetRecoService(recognition);
    }

    private static async Task<IRecognition> InitCloudRecognition()
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync("Ob5ONnctpgATb/SN9VUL32Hhe2YyMUR85DJlFke0lkc=");

        return cloudRecognition;
    }

    private static async Task<IRecognition> InitImageRecognition()
    {
        var imageRecognition = new ImageRecognition();

        var img1 = ImageData.Load("../../../img1.jpg");
        var img2 = ImageData.Load("../../../img2.jpg");
        var imgs = new List<ImageData>() { img1, img2 };

        await imageRecognition.InitAsync(imgs);

        return imageRecognition;
    }

    private static async Task<IRecognition> InitDatasetRecognition()
    {
        var imageRecognition = new ImageRecognition();
        var targetDataset = await TargetDataset.LoadAsync("../../../device-db.bin");
        await imageRecognition.InitAsync(targetDataset);

        return imageRecognition;
    }

    private void Camera_TrackFound(object sender, TargetMatchingEventArgs e)
    {
        foreach (var targetMatchResult in e.TargetMatchResults)
        {
            var points = Array.ConvertAll(targetMatchResult.ProjectedRegion, System.Drawing.Point.Round);
            using var vp = new VectorOfPoint(points);
            CvInvoke.Polylines(e.Frame, vp, true, new MCvScalar(255, 0, 0, 255), 5);

            var arrowpts = new System.Drawing.PointF[]
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

            float centerX = 0;
            float centerY = 0;

            foreach (var point in targetMatchResult.ProjectedRegion)
            {
                centerX += point.X;
                centerY += point.Y;
            }

            centerX /= targetMatchResult.ProjectedRegion.Length;
            centerY /= targetMatchResult.ProjectedRegion.Length;

            // Assume 'viewport' is a reference to a Viewport3D object
            // Assume 'centerX' and 'centerY' are the center point coordinates

            // Define the scale factor based on the size of the viewport
            var scaleFactorX = 2.0 / Camera.ActualWidth;
            var scaleFactorY = -2.0 / Camera.ActualHeight;

            // Scale the center point coordinates to fit within the viewport
            var scaledX = targetMatchResult.CenterX * scaleFactorX;
            var scaledY = targetMatchResult.CenterY * scaleFactorY;

            var data = targetMatchResult.HomographyArray.GetData();

            var rows = data.GetLength(0);
            var cols = data.GetLength(1);

            var homography = new Matrix<double>(rows, cols);
            for (var i = 0; i < rows; i++)
            {
                for (var j = 0; j < cols; j++)
                {
                    homography[i, j] = Convert.ToDouble(data.GetValue(i, j)!);
                }
            }

            var matrix3D = new Matrix3D(
                homography[0, 0],
                homography[0, 1],
                homography[0, 2],
                0,
                homography[1, 0],
                homography[1, 1],
                homography[1, 2],
                0,
                homography[2, 0],
                homography[2, 1],
                homography[2, 2],
                0,
                0,
                0,
                0,
                1);

            // extract the translation vector from the homography matrix
            var vector3D = new Vector3D(matrix3D.OffsetX, matrix3D.OffsetY, matrix3D.OffsetZ);

            translateTransform3D.OffsetX = scaledX / 10;
            translateTransform3D.OffsetY = scaledY / 10;
            translateTransform3D.OffsetZ = vector3D.Z;

            axisAngleRotation3D.Angle = -targetMatchResult.Angle;
            axisAngleRotation3D.Axis = new Vector3D(0, 0, 1);
        }
    }

    private static double CalculatePolygonAngle(System.Drawing.PointF[] points)
    {
        double totalAngle = 0;

        for (int i = 0; i < points.Length; i++)
        {
            System.Drawing.PointF currentPoint = points[i];
            System.Drawing.PointF prevPoint = i == 0 ? points[points.Length - 1] : points[i - 1];
            System.Drawing.PointF nextPoint = i == points.Length - 1 ? points[0] : points[i + 1];

            double angle = CalculateAngle(prevPoint, currentPoint, nextPoint);
            totalAngle += angle;
        }

        return totalAngle;
    }

    private static double CalculateAngle(System.Drawing.PointF a, System.Drawing.PointF b, System.Drawing.PointF c)
    {
        double angleRad = Math.Atan2(c.Y - b.Y, c.X - b.X) - Math.Atan2(a.Y - b.Y, a.X - b.X);
        double angleDeg = angleRad * (180 / Math.PI);

        if (angleDeg < 0)
        {
            angleDeg += 360;
        }

        return angleDeg;
    }

    private void Camera_TrackLost(object sender, System.EventArgs e)
    {
    }
}
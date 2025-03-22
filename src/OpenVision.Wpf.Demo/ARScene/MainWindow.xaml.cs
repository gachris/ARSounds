using DevToolbox.Wpf.Windows;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenVision.Core.Dataset;
using OpenVision.Core.Reco;
using OpenVision.Core.Reco.DataTypes;
using OpenVision.Wpf.Controls;
using System.Windows;
using System.Windows.Controls;

namespace OpenVision.Wpf.Demo.ARScene;

public partial class MainWindow : WindowEx
{
    private readonly MainWindowViewModel _viewModel;
    internal readonly List<ImageData> _loadedImages = [];

    public MainWindow()
    {
        _viewModel = new MainWindowViewModel(this);
        DataContext = _viewModel;

        InitializeComponent();
        RecoTypeComboBox.SelectionChanged += RecoTypeComboBox_SelectionChanged;
    }

    // Event handler for when a track is found
    private void Camera_TrackFound(object sender, TargetMatchingEventArgs e)
    {
        //_viewModel.TargetId = e.Id;

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

    private void Camera_TrackLost(object sender, EventArgs e)
    {
        _viewModel.TargetId = null;
    }

    private void RecoTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if ((RecoTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() == "Image Reco")
        {
            ImageRecoView.Visibility = Visibility.Visible;
        }
        else
        {
            ImageRecoView.Visibility = Visibility.Collapsed;
        }
    }

    // Event handler for "Start Recognition" button click
    private async void Start_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        string? selectedReco = (RecoTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
        if (selectedReco == "Image Reco")
        {
            if (_loadedImages.Count == 0)
            {
                MessageBox.Show("Please load images before starting recognition.");
                return;
            }
            await InitAndSetRecognitionService(() => InitImageRecognition(_loadedImages), "image");
        }
        else if (selectedReco == "Db Reco")
        {
            await InitAndSetRecognitionService(InitDatasetRecognition, "dataset");
        }
        else if (selectedReco == "Cloud Reco")
        {
            await InitAndSetRecognitionService(InitCloudRecognition, "cloud");
        }
    }

    // Helper method for initializing and setting recognition service
    private async Task InitAndSetRecognitionService(Func<Task<IRecognition>> recognitionInitFunc, string recoType)
    {
        try
        {
            DisableUI(true); // Disable UI controls during initialization
            var recognition = await recognitionInitFunc();
            SetRecognitionService(recognition);
            MessageBox.Show($"{recoType} recognition initialized successfully.");
        }
        catch (Exception ex)
        {
            LogError($"Failed to initialize {recoType} recognition", ex);
            MessageBox.Show($"Error initializing {recoType} recognition: {ex.Message}");
        }
        finally
        {
            DisableUI(false); // Enable UI controls after initialization
        }
    }

    // Method to set the recognition service to the AR camera
    private void SetRecognitionService(IRecognition recognition)
    {
        Camera.SetRecoService(recognition);
    }

    // Initialize cloud recognition
    private static async Task<IRecognition> InitCloudRecognition()
    {
        var cloudRecognition = new CloudRecognition();
        await cloudRecognition.InitAsync("o6a6bA3jFlUTn+YE6LnDsdDHrWWIDH6ppOE76jv84H4=");
        return cloudRecognition;
    }

    // Initialize image recognition
    private static async Task<IRecognition> InitImageRecognition(List<ImageData> loadedImages)
    {
        var imageRecognition = new ImageRecognition();
        await imageRecognition.InitAsync(loadedImages);
        return imageRecognition;
    }

    // Initialize dataset recognition
    private static async Task<IRecognition> InitDatasetRecognition()
    {
        var imageRecognition = new ImageRecognition();

        var dbUri = new Uri("pack://application:,,,/Vision.Wpf.Demo;component/Assets/device-db.bin");

        using var dbStream = Application.GetResourceStream(dbUri).Stream;

        var targetDataset = await TargetDataset.LoadAsync(dbStream);
        await imageRecognition.InitAsync(targetDataset);

        return imageRecognition;
    }

    // Helper method to log errors
    private void LogError(string message, Exception ex)
    {
        // Ideally log to a file or a logging system
        Console.WriteLine($"{message}: {ex}");
    }

    // Helper method to disable or enable UI during recognition setup
    private void DisableUI(bool isDisabled)
    {
        RecoTypeComboBox.IsEnabled = !isDisabled;
        StartRecoButton.IsEnabled = !isDisabled;
    }
}

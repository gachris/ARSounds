using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DevToolbox.Wpf.Windows;
using Microsoft.ML;
using Microsoft.ML.Data;
using OpenVision.Wpf.Controls;
using Rectangle = System.Windows.Shapes.Rectangle;
using OpenVision.Wpf.Demo.ML.DataModels;

namespace OpenVision.Wpf.Demo.ML;

public partial class MainWindow : WindowEx
{
    private OnnxOutputParser? _outputParser;
    private PredictionEngine<ImageInputData, TinyYoloPrediction>? _tinyYoloPredictionEngine;
    private PredictionEngine<ImageInputData, CustomVisionPrediction>? _customVisionPredictionEngine;

    private static readonly string modelsDirectory = Path.Combine(Environment.CurrentDirectory, @"Assets\OnnxModels");

    public MainWindow()
    {
        InitializeComponent();
        LoadModel();

        Camera.FrameChanged += Camera_FrameChanged;
    }

    private void LoadModel()
    {
        // Check for an Onnx model exported from Custom Vision
        var customVisionExport = Directory.GetFiles(modelsDirectory, "*.zip").FirstOrDefault();

        // If there is one, use it.
        if (customVisionExport != null)
        {
            var customVisionModel = new CustomVisionModel(customVisionExport);
            var modelConfigurator = new OnnxModelConfigurator(customVisionModel);

            _outputParser = new OnnxOutputParser(customVisionModel);
            _customVisionPredictionEngine = modelConfigurator.GetMlNetPredictionEngine<CustomVisionPrediction>();
        }
        else // Otherwise default to Tiny Yolo Onnx model
        {
            var tinyYoloModel = new TinyYoloModel(Path.Combine(modelsDirectory, "tinyyolov2-8.onnx"));
            var modelConfigurator = new OnnxModelConfigurator(tinyYoloModel);

            _outputParser = new OnnxOutputParser(tinyYoloModel);
            _tinyYoloPredictionEngine = modelConfigurator.GetMlNetPredictionEngine<TinyYoloPrediction>();
        }
    }

    private async void Camera_FrameChanged(object? sender, FrameChangedEventArgs e)
    {
        using var memoryStream = new MemoryStream(e.Frame);
        memoryStream.Seek(0, SeekOrigin.Begin);
        var mLImage = MLImage.CreateFromStream(memoryStream);

        if (_customVisionPredictionEngine == null && _tinyYoloPredictionEngine == null)
            return;

        var frame = new ImageInputData { Image = mLImage };
        var filteredBoxes = DetectObjectsUsingModel(frame);

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            DrawOverlays(filteredBoxes, mLImage.Height, mLImage.Width);
        });
    }

    public List<BoundingBox> DetectObjectsUsingModel(ImageInputData imageInputData)
    {
        ArgumentNullException.ThrowIfNull(_outputParser, nameof(_outputParser));
        ArgumentNullException.ThrowIfNull(_tinyYoloPredictionEngine, nameof(_tinyYoloPredictionEngine));

        var labels = _tinyYoloPredictionEngine.Predict(imageInputData).PredictedLabels ?? _tinyYoloPredictionEngine?.Predict(imageInputData).PredictedLabels;
        ArgumentNullException.ThrowIfNull(labels, nameof(labels));

        var boundingBoxes = _outputParser.ParseOutputs(labels);
        var filteredBoxes = _outputParser.FilterBoundingBoxes(boundingBoxes, 5, 0.5f);
        return filteredBoxes;
    }

    private void DrawOverlays(List<BoundingBox> filteredBoxes, double originalHeight, double originalWidth)
    {
        WebCamCanvas.Children.Clear();

        foreach (var box in filteredBoxes)
        {
            ArgumentNullException.ThrowIfNull(box.Dimensions, nameof(box.Dimensions));

            // process output boxes
            double x = Math.Max(box.Dimensions.X, 0);
            double y = Math.Max(box.Dimensions.Y, 0);
            double width = Math.Min(originalWidth - x, box.Dimensions.Width);
            double height = Math.Min(originalHeight - y, box.Dimensions.Height);

            // fit to current image size
            x = originalWidth * x / ImageSettings.imageWidth;
            y = originalHeight * y / ImageSettings.imageHeight;
            width = originalWidth * width / ImageSettings.imageWidth;
            height = originalHeight * height / ImageSettings.imageHeight;

            var boxColor = box.BoxColor.ToMediaColor();

            var objBox = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(Colors.Transparent),
                Stroke = new SolidColorBrush(boxColor),
                StrokeThickness = 2.0,
                Margin = new Thickness(x, y, 0, 0)
            };

            var objDescription = new TextBlock
            {
                Margin = new Thickness(x + 4, y + 4, 0, 0),
                Text = box.Description,
                FontWeight = FontWeights.Bold,
                Width = 126,
                Height = 21,
                TextAlignment = TextAlignment.Center
            };

            var objDescriptionBackground = new Rectangle
            {
                Width = 134,
                Height = 29,
                Fill = new SolidColorBrush(boxColor),
                Margin = new Thickness(x, y, 0, 0)
            };

            WebCamCanvas.Children.Add(objDescriptionBackground);
            WebCamCanvas.Children.Add(objDescription);
            WebCamCanvas.Children.Add(objBox);
        }
    }
}

internal static class ColorExtensions
{
    internal static Color ToMediaColor(this System.Drawing.Color drawingColor)
    {
        return Color.FromArgb(drawingColor.A, drawingColor.R, drawingColor.G, drawingColor.B);
    }
}
using OpenVision.ML.Maui.Demo.ML;
using OpenVision.ML.Maui.Demo.ML.Models.Mobilenet;
using OpenVision.ML.Maui.Demo.ML.Models.TinyYolo;
using OpenVision.ML.Maui.Demo.ML.Models.Ultraface;
using OpenVision.ML.Maui.Demo.PrePostProcessing;
using OpenVision.Maui.Controls;

namespace OpenVision.ML.Maui.Demo;

public partial class MainPage : ContentPage
{
    private IVisionSample _mobilenet;
    private IVisionSample _tinyYolo;
    private IVisionSample _ultraface;

    private IVisionSample TinyYolo => _tinyYolo ??= new TinyYoloVision();

    private IVisionSample Mobilenet => _mobilenet ??= new MobilenetVision();

    private IVisionSample Ultraface => _ultraface ??= new UltrafaceVision();

    public MainPage()
    {
        InitializeComponent();

        // See:
        // ONNX Runtime Execution Providers: https://onnxruntime.ai/docs/execution-providers/
        // Core ML: https://developer.apple.com/documentation/coreml
        // NNAPI: https://developer.android.com/ndk/guides/neuralnetworks
        ExecutionProviderOptions.Items.Add(nameof(ExecutionProviders.CPU));

        if (DeviceInfo.Platform == DevicePlatform.Android)
        {
            ExecutionProviderOptions.Items.Add(nameof(ExecutionProviders.NNAPI));
        }

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            ExecutionProviderOptions.Items.Add(nameof(ExecutionProviders.CoreML));
        }

        ExecutionProviderOptions.SelectedIndex = 0;

        if (FileSystem.Current.AppPackageFileExistsAsync(TinyYoloVision.ModelFilename).Result)
        {
            Models.Items.Add(TinyYolo.Name);
        }

        if (FileSystem.Current.AppPackageFileExistsAsync(MobilenetVision.ModelFilename).Result)
        {
            Models.Items.Add(Mobilenet.Name);
        }

        if (FileSystem.Current.AppPackageFileExistsAsync(UltrafaceVision.ModelFilename).Result)
        {
            Models.Items.Add(Ultraface.Name);
        }

        if (Models.Items.Any())
        {
            Models.SelectedIndex = Models.Items.IndexOf(Models.Items.First());
        }
        else
        {
            Models.IsEnabled = false;
        }

        Camera.FrameChanged += Camera_FrameChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ExecutionProviderOptions.SelectedIndexChanged += ExecutionProviderOptions_SelectedIndexChanged;
        Models.SelectedIndexChanged += Models_SelectedIndexChanged;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        ExecutionProviderOptions.SelectedIndexChanged -= ExecutionProviderOptions_SelectedIndexChanged;
        Models.SelectedIndexChanged -= Models_SelectedIndexChanged;
    }

    private async Task UpdateExecutionProviderAsync()
    {
        var executionProvider = ExecutionProviderOptions.SelectedItem switch
        {
            nameof(ExecutionProviders.CPU) => ExecutionProviders.CPU,
            nameof(ExecutionProviders.NNAPI) => ExecutionProviders.NNAPI,
            nameof(ExecutionProviders.CoreML) => ExecutionProviders.CoreML,
            _ => ExecutionProviders.CPU
        };

        IVisionSample sample = Models.SelectedItem switch
        {
            MobilenetVision.Identifier => Mobilenet,
            UltrafaceVision.Identifier => Ultraface,
            TinyYoloVision.Identifier => Ultraface,
            _ => null
        };

        await sample.UpdateExecutionProviderAsync(executionProvider);
    }

    private void SetBusyState(bool busy)
    {
    }

    private void ExecutionProviderOptions_SelectedIndexChanged(object sender, EventArgs e)
        => UpdateExecutionProviderAsync().ContinueWith((task)
            =>
        {
            if (task.IsFaulted) MainThread.BeginInvokeOnMainThread(()
            => DisplayAlert("Error", task.Exception.Message, "OK"));
        });

    private void Models_SelectedIndexChanged(object sender, EventArgs e)
        // make sure EP is in sync
        => ExecutionProviderOptions_SelectedIndexChanged(null, null);

    private async void Camera_FrameChanged(object sender, FrameChangedEventArgs e)
    {
        byte[] outputImage = null;
        string caption = null;
        TinyYoloPrediction[] tinyYoloPredictions = null;

        try
        {
            SetBusyState(true);

            if (Models.Items.Count == 0 || Models.SelectedItem == null)
            {
                SetBusyState(false);
                await DisplayAlert("No Samples", "Model files could not be found", "OK");
                return;
            }

            var imageData = e.Frame;

            if (imageData == null)
            {
                SetBusyState(false);
                return;
            }

            ClearResult();

            IVisionSample sample = Models.SelectedItem switch
            {
                MobilenetVision.Identifier => Mobilenet,
                UltrafaceVision.Identifier => Ultraface,
                TinyYoloVision.Identifier => TinyYolo,
                _ => null
            };

            var result = await sample.ProcessImageAsync(imageData);

            if (result is TinyYoloImageProcessingResult tinyYoloImageProcessingResult)
            {
                tinyYoloPredictions = tinyYoloImageProcessingResult.Predictions;
            }

            outputImage = result.Image;
            caption = result.Caption;
        }
        finally
        {
            SetBusyState(false);
        }

        if (outputImage != null)
        {
            ShowResult(outputImage, caption);
        }

        if (tinyYoloPredictions != null)
        {
            DrawOverlays(tinyYoloPredictions, Camera.Height, Camera.Width);
        }
    }

    private void ClearResult()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            //OutputImage.Source = null;
            Caption.Text = string.Empty;

            WebCamStackLayout.Children.Clear();
        });
    }

    private void ShowResult(byte[] image, string caption = null)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            //OutputImage.Source = ImageSource.FromStream(() => new MemoryStream(image));
            Caption.Text = string.IsNullOrWhiteSpace(caption) ? string.Empty : caption;
        });
    }

    private void DrawOverlays(TinyYoloPrediction[] filteredBoxes, double originalHeight, double originalWidth)
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            WebCamStackLayout.Children.Clear();

            foreach (var box in filteredBoxes)
            {
                // process output boxes
                var x = (double)Math.Max(box.Dimensions.X, 0);
                var y = (double)Math.Max(box.Dimensions.Y, 0);
                var width = Math.Min(originalWidth - x, box.Dimensions.Width);
                var height = Math.Min(originalHeight - y, box.Dimensions.Height);

                // fit to current image size
                x = originalWidth * x / ImageSettings.ImageWidth;
                y = originalHeight * y / ImageSettings.ImageHeight;
                width = originalWidth * width / ImageSettings.ImageWidth;
                height = originalHeight * height / ImageSettings.ImageHeight;

                var boxColor = Color.FromRgba(box.BoxColor.R, box.BoxColor.G, box.BoxColor.B, box.BoxColor.A);

                var objBox = new Border
                {
                    WidthRequest = width,
                    HeightRequest = height,
                    BackgroundColor = Color.FromRgba(0, 0, 0, 0),
                    Stroke = boxColor,
                    StrokeThickness = 2.0,
                };

                var objDescription = new Label
                {
                    Text = box.Description,
                    FontAttributes = FontAttributes.Bold,
                    WidthRequest = 126,
                    HeightRequest = 21,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

                var objDescriptionBackground = new BoxView
                {
                    WidthRequest = 134,
                    HeightRequest = 29,
                    Color = boxColor
                };

                AbsoluteLayout.SetLayoutBounds(objDescriptionBackground, new Rect(x, y, objDescriptionBackground.Width, objDescriptionBackground.Height));
                AbsoluteLayout.SetLayoutBounds(objDescription, new Rect(x, y, objDescription.Width, objDescription.Height));
                AbsoluteLayout.SetLayoutBounds(objBox, new Rect(x, y, width, height));

                WebCamStackLayout.Children.Add(objDescriptionBackground);
                WebCamStackLayout.Children.Add(objDescription);
                WebCamStackLayout.Children.Add(objBox);
            }
        });
    }
}
using System.Text;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using OpenVision.ML.Maui.Demo.PrePostProcessing;

namespace OpenVision.ML.Maui.Demo.ML.Models.TinyYolo;

public class TinyYoloVision : VisionSampleBase<TinyYoloImageProcessor>
{
    private static readonly (float, float)[] Anchors = { (1.08f, 1.19f), (3.42f, 4.41f), (6.63f, 11.38f), (9.42f, 5.11f), (16.62f, 10.52f) };

    public const string Identifier = "TinyYoloSample V2";
    public const string ModelFilename = "TinyYolo2_model.onnx";

    private readonly TinyYoloOutputParser _outputParser;

    public TinyYoloVision() : base(Identifier, ModelFilename)
    {
        _outputParser = new TinyYoloOutputParser(TinyYoloLabelMap.Labels, Anchors);
    }

    protected override async Task<ImageProcessingResult> OnProcessImageAsync(byte[] image)
    {
        // Resize and center crop
        using var preprocessedImage = await Task.Run(() => ImageProcessor.PreprocessSourceImage(image)).ConfigureAwait(false);

        // Convert to Tensor of normalized float RGB data with NCHW ordering
        var tensor = await Task.Run(() => ImageProcessor.GetTensorForImage(preprocessedImage)).ConfigureAwait(false);

        // Run the model
        var predictions = await Task.Run(() => GetPredictions(tensor)).ConfigureAwait(false);

        // Get the pre-processed image for display to the user so they can see the actual input to the model
        var preprocessedImageData = await Task.Run(() => ImageProcessor.GetBytesForBitmap(preprocessedImage)).ConfigureAwait(false);

        var caption = string.Empty;

        if (predictions.Any())
        {
            var builder = new StringBuilder();

            if (predictions.Any())
            {
                builder.Append($"Top {predictions.Count} predictions: {Environment.NewLine}{Environment.NewLine}");
            }

            foreach (var prediction in predictions)
            {
                builder.Append($"{prediction.Label} ({prediction.Confidence * 100:0.00}%){Environment.NewLine}");
            }

            caption = builder.ToString();
        }

        return new TinyYoloImageProcessingResult(preprocessedImageData, predictions.ToArray(), caption);
    }

    private List<TinyYoloPrediction> GetPredictions(Tensor<float> input)
    {
        // Setup inputs and outputs
        var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("image", input) };

        // Run inference
        using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = Session.Run(inputs);

        // Postprocess to get softmax vector
        var modelOutput = results.First().AsEnumerable<float>().ToArray();
        var boundingBoxes = _outputParser.ParseOutputs(modelOutput);
        var filteredBoxes = _outputParser.FilterBoundingBoxes(boundingBoxes, 5, 0.5f);

        return filteredBoxes;
    }
}

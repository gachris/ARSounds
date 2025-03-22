using Microsoft.ML.OnnxRuntime.Tensors;
using OpenVision.ML.Maui.Demo.PrePostProcessing;
using SkiaSharp;

namespace OpenVision.ML.Maui.Demo.ML.Models.TinyYolo;

public class TinyYoloImageProcessor : SkiaSharpImageProcessor<TinyYoloPrediction, float>
{
    private const int RequiredHeight = 416;
    private const int RequiredWidth = 416;

    protected override SKBitmap OnPreprocessSourceImage(SKBitmap sourceImage)
        => sourceImage.Resize(new SKImageInfo(RequiredWidth, RequiredHeight), SKFilterQuality.High);

    protected override Tensor<float> OnGetTensorForImage(SKBitmap image)
    {
        var bytes = image.GetPixelSpan();

        // For Ultraface, the expected input would be 320 x 240 x 4 (in RGBA format)
        var expectedInputLength = RequiredWidth * RequiredHeight * 4;

        // For the Tensor, we need 3 channels so 320 x 240 x 3 (in RGB format)
        var expectedOutputLength = RequiredWidth * RequiredHeight * 3;

        if (bytes.Length != expectedInputLength)
        {
            throw new Exception($"The parameter {nameof(image)} is an unexpected length. " +
                                $"Expected length is {expectedInputLength}");
        }

        // The channelData array is expected to be in RGB order with a mean applied i.e. (pixel - 127.0f) / 128.0f
        float[] channelData = new float[expectedOutputLength];

        // Extract only the desired channel data (don't want the alpha)
        var expectedChannelLength = expectedOutputLength / 3;
        var redOffset = expectedChannelLength * 0;
        var greenOffset = expectedChannelLength * 1;
        var blueOffset = expectedChannelLength * 2;

        for (int i = 0, i2 = 0; i < bytes.Length; i += 4, i2++)
        {
            var r = Convert.ToSingle(bytes[i]);
            var g = Convert.ToSingle(bytes[i + 1]);
            var b = Convert.ToSingle(bytes[i + 2]);
            channelData[i2 + redOffset] = r;
            channelData[i2 + greenOffset] = g;
            channelData[i2 + blueOffset] = b;
        }

        return new DenseTensor<float>(new Memory<float>(channelData),
                                      new[] { 1, 3, RequiredHeight, RequiredWidth });
    }
}
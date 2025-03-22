using OpenVision.ML.Maui.Demo.ML.Models.TinyYolo;

namespace OpenVision.ML.Maui.Demo.PrePostProcessing;

public class TinyYoloImageProcessingResult : ImageProcessingResult
{
    public TinyYoloPrediction[] Predictions { get; }

    internal TinyYoloImageProcessingResult(byte[] image, string caption = null) : base(image, caption)
    {
    }

    internal TinyYoloImageProcessingResult(byte[] image, TinyYoloPrediction[] predictions, string caption = null) : base(image, caption)
    {
        Predictions = predictions;
    }
}
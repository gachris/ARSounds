using Microsoft.ML.OnnxRuntime.Tensors;

namespace OpenVision.ML.Maui.Demo.PrePostProcessing;

public interface IImageProcessor<TImage, TPrediction, TTensor>
{
    // pre-process the image.
    TImage PreprocessSourceImage(byte[] sourceImage);

    // convert the PreprocessSourceImage output to a Tensor of type TTensor 
    Tensor<TTensor> GetTensorForImage(TImage image);

    // apply the predictions to the image if applicable
    // e.g. draw the bounding box around an area selected by the model
    byte[] ApplyPredictionsToImage(IList<TPrediction> predictions, TImage image);
}
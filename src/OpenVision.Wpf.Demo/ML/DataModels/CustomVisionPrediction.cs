using Microsoft.ML.Data;

namespace OpenVision.Wpf.Demo.ML.DataModels;

public class CustomVisionPrediction : IOnnxObjectPrediction
{
    [ColumnName("model_outputs0")]
    public float[]? PredictedLabels { get; set; }
}

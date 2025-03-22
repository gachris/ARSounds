using Microsoft.ML.Data;

namespace OpenVision.Wpf.Demo.ML.DataModels;

public class TinyYoloPrediction : IOnnxObjectPrediction
{
    [ColumnName("grid")]
    public float[]? PredictedLabels { get; set; }
}

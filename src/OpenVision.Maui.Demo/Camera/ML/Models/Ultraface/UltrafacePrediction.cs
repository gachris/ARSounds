using OpenVision.ML.Maui.Demo.PrePostProcessing;

namespace OpenVision.ML.Maui.Demo.ML.Models.Ultraface;

public class UltrafacePrediction
{
    public PredictionBox Box { get; set; }

    public float Confidence { get; set; }
}
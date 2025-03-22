using System.Drawing;

namespace OpenVision.ML.Maui.Demo.ML.Models.TinyYolo;

public class BoundingBoxDimensions
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Height { get; set; }
    public float Width { get; set; }
}

public class TinyYoloPrediction
{
    public BoundingBoxDimensions Dimensions { get; set; }

    public string Label { get; set; }

    public float Confidence { get; set; }

    public RectangleF Rect
    {
        get { return new RectangleF(Dimensions.X, Dimensions.Y, Dimensions.Width, Dimensions.Height); }
    }

    public System.Drawing.Color BoxColor { get; set; }

    public string Description => $"{Label} ({Confidence * 100:0}%)";

    private static readonly System.Drawing.Color[] Colors = new System.Drawing.Color[]
    {
        System.Drawing.Color.Khaki, System.Drawing.Color.Fuchsia, System.Drawing.Color.Silver, System.Drawing.Color.RoyalBlue,
        System.Drawing.Color.Green, System.Drawing.Color.DarkOrange, System.Drawing.Color.Purple, System.Drawing.Color.Gold,
        System.Drawing.Color.Red, System.Drawing.Color.Aquamarine, System.Drawing.Color.Lime, System.Drawing.Color.AliceBlue,
        System.Drawing.Color.Sienna, System.Drawing.Color.Orchid, System.Drawing.Color.Tan, System.Drawing.Color.LightPink,
        System.Drawing.Color.Yellow, System.Drawing.Color.HotPink, System.Drawing.Color.OliveDrab, System.Drawing.Color.SandyBrown,
        System.Drawing.Color.DarkTurquoise
    };

    public static System.Drawing.Color GetColor(int index) => index < Colors.Length ? Colors[index] : Colors[index % Colors.Length];
}
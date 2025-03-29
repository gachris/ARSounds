namespace ARSounds.UI.Common.Data;

public class TargetMatchingResult
{
    public string Id { get; }

    public System.Drawing.PointF[] ProjectedRegion { get; }

    public System.Drawing.SizeF Size { get; }

    public float CenterX { get; }

    public float CenterY { get; }

    public float Angle { get; }

    internal TargetMatchingResult(string id,
                                  System.Drawing.PointF[] projectedRegion,
                                  System.Drawing.SizeF size,
                                  float centerX,
                                  float centerY,
                                  float angle)
    {
        Id = id;
        ProjectedRegion = projectedRegion;
        Size = size;
        CenterX = centerX;
        CenterY = centerY;
        Angle = angle;
    }
}

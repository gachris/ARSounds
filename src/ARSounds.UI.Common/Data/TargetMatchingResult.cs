#if WINDOWS
using Emgu.CV;
#else
using OpenCV.Core;
#endif
using OpenVision.Core.DataTypes;

namespace ARSounds.UI.Common.Data;

public class TargetMatchingResult
{
    public Mat Frame { get; }

    public IReadOnlyCollection<TargetMatchResult> TargetMatchResults { get; }

    public TargetMatchingResult(Mat frame, IReadOnlyCollection<TargetMatchResult> targetMatchResults)
    {
        Frame = frame;
        TargetMatchResults = targetMatchResults;
    }
}
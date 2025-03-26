using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using OpenVision.Core.DataTypes;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ARSounds.UI.WinUI.Camera.Views;

internal partial class ARCameraHelper
{
    public static Image<Bgra, byte> DecodeBase64(string imageBase64)
    {
        // Remove the data URL header if present, then decode
        var imageBytes = Convert.FromBase64String(
            ImageDataUrlBase64PrefixRegex().Replace(imageBase64, "")
        );

        var fullWaveMat = new Mat();
        CvInvoke.Imdecode(imageBytes, ImreadModes.Unchanged, fullWaveMat);
        return fullWaveMat.ToImage<Bgra, byte>();
    }

    public static void UpdateOverlayImage(
        Image<Bgra, byte> waveformImage,
        Mat cameraFrame,
        TargetMatchResult targetMatchResult,
        double audioProgress,
        string hexColor)
    {
        // Ensure the camera frame has an alpha channel (4 channels)
        EnsureCameraFrameHasAlpha(cameraFrame);

        // 1) Decode the full waveform image from Base64
        var fullWidth = waveformImage.Width;

        // 2) Crop a 20px wide slice based on the audio progress
        var (sliceImage, sliceLeft) = CropWaveformSlice(waveformImage, audioProgress);

        // 3) Replace any black pixels with the provided hex color
        ReplaceBlackPixels(sliceImage, hexColor);

        // 4) Calculate the destination corners for the slice using the target's projected region
        var dstCorners = CalculateDestinationCorners(targetMatchResult, sliceLeft, 20, fullWidth);
        if (dstCorners == null)
        {
            return;
        }

        // 5) Define source corners from the slice image
        var srcCorners = GetSourceCorners(sliceImage);

        // 6) Apply the perspective transform so the slice maps to the computed destination region
        using var transformedSlice = ApplyPerspectiveTransform(sliceImage, srcCorners, dstCorners, cameraFrame.Size);

        // 7) Blend the transformed slice onto the camera frame
        BlendImages(cameraFrame, transformedSlice);
    }

    private static void EnsureCameraFrameHasAlpha(Mat cameraFrame)
    {
        if (cameraFrame.NumberOfChannels != 4)
        {
            CvInvoke.CvtColor(cameraFrame, cameraFrame, ColorConversion.Bgr2Bgra);
        }
    }

    private static (Image<Bgra, byte> sliceImage, int sliceLeft) CropWaveformSlice(Image<Bgra, byte> fullWaveImage, double audioProgress)
    {
        var fullWidth = fullWaveImage.Width;
        var fullHeight = fullWaveImage.Height;
        const int sliceWidth = 20;

        // Calculate horizontal offset ensuring it is within bounds
        var sliceLeft = (int)(audioProgress * (fullWidth - sliceWidth));
        sliceLeft = Math.Max(0, Math.Min(sliceLeft, fullWidth - sliceWidth));

        var cropRect = new Rectangle(sliceLeft, 0, sliceWidth, fullHeight);
        var sliceImage = fullWaveImage.Copy(cropRect);
        return (sliceImage, sliceLeft);
    }

    private static void ReplaceBlackPixels(Image<Bgra, byte> image, string hexColor)
    {
        var color = ColorTranslator.FromHtml(hexColor);
        for (var y = 0; y < image.Height; y++)
        {
            for (var x = 0; x < image.Width; x++)
            {
                var pixel = image[y, x];
                // If pixel is exactly black and not fully transparent, replace it with the hex color
                if (pixel.Blue == 0 && pixel.Green == 0 && pixel.Red == 0 && pixel.Alpha > 0)
                {
                    image[y, x] = new Bgra(color.B, color.G, color.R, pixel.Alpha);
                }
            }
        }
    }

    private static PointF[]? CalculateDestinationCorners(TargetMatchResult targetMatchResult, int sliceLeft, int sliceWidth, int fullWidth)
    {
        var fractionLeft = sliceLeft / (double)fullWidth;
        var fractionRight = (sliceLeft + sliceWidth) / (double)fullWidth;

        var corners = targetMatchResult.ProjectedRegion;
        if (corners.Length < 4)
        {
            return null; // Not enough points to define the region.
        }

        // Assuming order: top-left, top-right, bottom-right, bottom-left
        var p1 = corners[0];
        var p2 = corners[1];
        var p3 = corners[2];
        var p4 = corners[3];

        var topLeft = InterpolatePoint(p1, p2, fractionLeft);
        var topRight = InterpolatePoint(p1, p2, fractionRight);
        var bottomRight = InterpolatePoint(p4, p3, fractionRight);
        var bottomLeft = InterpolatePoint(p4, p3, fractionLeft);

        return [topLeft, topRight, bottomRight, bottomLeft];
    }

    private static PointF[] GetSourceCorners(Image<Bgra, byte> sliceImage)
    {
        return
        [
            new PointF(0, 0),
            new PointF(sliceImage.Width - 1, 0),
            new PointF(sliceImage.Width - 1, sliceImage.Height - 1),
            new PointF(0, sliceImage.Height - 1)
        ];
    }

    private static Mat ApplyPerspectiveTransform(Image<Bgra, byte> sliceImage, PointF[] srcCorners, PointF[] dstCorners, Size targetSize)
    {
        var transformedSlice = new Mat(targetSize, DepthType.Cv8U, 4);
        transformedSlice.SetTo(new MCvScalar(0, 0, 0, 0));

        using var matrix = CvInvoke.GetPerspectiveTransform(srcCorners, dstCorners);
        CvInvoke.WarpPerspective(sliceImage, transformedSlice, matrix, targetSize,
                                 Inter.Linear, Warp.Default, BorderType.Constant);
        return transformedSlice;
    }

    /// <summary>
    /// Interpolates along a single edge (e.g. between the top two points or bottom two points)
    /// for a given horizontal fraction [0..1].
    /// </summary>
    private static PointF InterpolatePoint(PointF start, PointF end, double fraction)
    {
        var x = (float)(start.X + (end.X - start.X) * fraction);
        var y = (float)(start.Y + (end.Y - start.Y) * fraction);
        return new PointF(x, y);
    }

    private static void BlendImages(Mat background, Mat foreground)
    {
        var channels = new VectorOfMat();
        CvInvoke.Split(foreground, channels);

        var alpha = channels[3];
        var maskInv = new Mat();
        CvInvoke.BitwiseNot(alpha, maskInv);

        var backgroundRegion = new Mat();
        CvInvoke.BitwiseAnd(background, background, backgroundRegion, maskInv);

        var foregroundRegion = new Mat();
        CvInvoke.BitwiseAnd(foreground, foreground, foregroundRegion, alpha);

        CvInvoke.Add(backgroundRegion, foregroundRegion, background);
    }

    [GeneratedRegex("^data:image/[^;]+;base64,")]
    private static partial Regex ImageDataUrlBase64PrefixRegex();
}
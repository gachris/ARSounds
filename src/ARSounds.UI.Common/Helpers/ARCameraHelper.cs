using System.Text.RegularExpressions;
#if WINDOWS
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
#else
using OpenCV.Core;
using OpenCV.ImgProc;
using OpenVision.Core.Utils;
#endif
using OpenVision.Core.DataTypes;
using PointF = System.Drawing.PointF;
using Size = System.Drawing.Size;

namespace ARSounds.UI.Common.Camera;

public partial class ARCameraHelper
{
#if WINDOWS
    public static Image<Bgra, byte> DecodeBase64(string imageBase64)
    {
        // Remove the data URL header if present, then decode
        var imageBytes = Convert.FromBase64String(
            ImageDataUrlBase64PrefixRegex().Replace(imageBase64, string.Empty)
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
#else
    /// <summary>
    /// Removes any data URL header and decodes the Base64 image.
    /// On Windows, returns a Mat from Emgu; on non‑Windows it returns an OpenCvSharp.Mat.
    /// </summary>
    public static Mat DecodeBase64(string imageBase64)
    {
        var imageBytes = Convert.FromBase64String(
            ImageDataUrlBase64PrefixRegex().Replace(imageBase64, "")
        );
        return imageBytes.ToMat();
    }

    /// <summary>
    /// Updates the overlay on the camera frame by cropping a slice from the waveform image,
    /// replacing black pixels with the provided hex color, warping it to the destination region,
    /// and blending it into the camera frame.
    /// </summary>
    public static void UpdateOverlayImage(
        Mat waveformImage,
        Mat cameraFrame,
        TargetMatchResult targetMatchResult,
        double audioProgress,
        string hexColor)
    {
        // 1) Ensure the camera frame has an alpha channel (4 channels)
        EnsureCameraFrameHasAlpha(cameraFrame);

        // 2) Get the full width of the waveform image.

        int fullWidth = waveformImage.Width();
        // On non-Windows, we work with Mat directly (using OpenCvSharp)
        var (sliceMat, sliceLeft) = CropWaveformSlice(waveformImage, audioProgress);
        ReplaceBlackPixels(sliceMat, hexColor);
        var dstCorners = CalculateDestinationCorners(targetMatchResult, sliceLeft, sliceMat.Width(), fullWidth);
        if (dstCorners == null) return;
        var srcCorners = GetSourceCorners(sliceMat);
        using var transformedSlice = ApplyPerspectiveTransform(sliceMat, srcCorners, dstCorners, new Size((int)cameraFrame.Size().Width, (int)cameraFrame.Size().Height));
        BlendImages(cameraFrame, transformedSlice);
    }

    #region Ensure CameraFrame Has Alpha

    private static void EnsureCameraFrameHasAlpha(Mat cameraFrame)
    {
        if (cameraFrame.Channels() != 4)
        {
            Imgproc.CvtColor(cameraFrame, cameraFrame, Imgproc.ColorBgr2bgra);
        }
    }

    #endregion

    private static (Mat sliceMat, int sliceLeft) CropWaveformSlice(Mat fullWaveMat, double audioProgress)
    {
        int fullWidth = fullWaveMat.Width();
        int fullHeight = fullWaveMat.Height();
        const int sliceWidth = 20;
        int sliceLeft = (int)(audioProgress * (fullWidth - sliceWidth));
        sliceLeft = Math.Max(0, Math.Min(sliceLeft, fullWidth - sliceWidth));
        var cropRect = new OpenCV.Core.Rect(sliceLeft, 0, sliceWidth, fullHeight);
        Mat sliceMat = new Mat(fullWaveMat, cropRect);
        return (sliceMat, sliceLeft);
    }

    private static void ReplaceBlackPixels(Mat image, string hexColor)
    {
        var color = System.Drawing.ColorTranslator.FromHtml(hexColor);
        // Loop through each pixel. (For a narrow slice, performance should be acceptable.)
        for (int y = 0; y < image.Rows(); y++)
        {
            for (int x = 0; x < image.Cols(); x++)
            {
                double[] pixel = image.Get(y, x); // Expected order: BGRA
                if (pixel[0] == 0 && pixel[1] == 0 && pixel[2] == 0 && pixel[3] > 0)
                {
                    image.Put(y, x, [color.B, color.G, color.R, pixel[3]]);
                }
            }
        }
    }

    /// <summary>
    /// Computes the destination corners (as PointF[]) for the slice based on the target’s projected region.
    /// Returns null if there aren’t at least 4 points.
    /// </summary>
    private static PointF[]? CalculateDestinationCorners(TargetMatchResult targetMatchResult, int sliceLeft, int sliceWidth, int fullWidth)
    {
        double fractionLeft = sliceLeft / (double)fullWidth;
        double fractionRight = (sliceLeft + sliceWidth) / (double)fullWidth;

        var corners = targetMatchResult.ProjectedRegion;
        if (corners.Length < 4) return null;

        // Assume order: top-left, top-right, bottom-right, bottom-left
        PointF p1 = corners[0];
        PointF p2 = corners[1];
        PointF p3 = corners[2];
        PointF p4 = corners[3];

        PointF topLeft = InterpolatePoint(p1, p2, fractionLeft);
        PointF topRight = InterpolatePoint(p1, p2, fractionRight);
        PointF bottomRight = InterpolatePoint(p4, p3, fractionRight);
        PointF bottomLeft = InterpolatePoint(p4, p3, fractionLeft);

        return new PointF[] { topLeft, topRight, bottomRight, bottomLeft };
    }

    private static PointF[] GetSourceCorners(Mat sliceMat)
    {
        return new PointF[]
        {
            new PointF(0,0),
            new PointF(sliceMat.Width() - 1, 0),
            new PointF(sliceMat.Width() - 1, sliceMat.Height() - 1),
            new PointF(0, sliceMat.Height() - 1)
        };
    }

    /// <summary>
    /// Interpolates between two points based on the given fraction [0..1].
    /// </summary>
    private static PointF InterpolatePoint(PointF start, PointF end, double fraction)
    {
        float x = (float)(start.X + (end.X - start.X) * fraction);
        float y = (float)(start.Y + (end.Y - start.Y) * fraction);
        return new PointF(x, y);
    }

    private static Mat ApplyPerspectiveTransform(Mat sliceMat, PointF[] srcCorners, PointF[] dstCorners, Size targetSize)
    {
        // Convert PointF[] to MatOfPoint2f (Android OpenCV SDK)
        var srcPoints = new MatOfPoint2f(srcCorners.Select(p => new OpenCV.Core.Point(p.X, p.Y)).ToArray());
        var dstPoints = new MatOfPoint2f(dstCorners.Select(p => new OpenCV.Core.Point(p.X, p.Y)).ToArray());
        Mat matrix = Imgproc.GetPerspectiveTransform(srcPoints, dstPoints);
        Mat transformedSlice = new Mat();
        // Use the targetSize (convert to Org.Opencv.Core.Size)
        OpenCV.Core.Size dSize = new OpenCV.Core.Size(targetSize.Width, targetSize.Height);
        Imgproc.WarpPerspective(sliceMat, transformedSlice, matrix, dSize, Imgproc.InterLinear, OpenCV.Core.Core.BorderConstant);
        return transformedSlice;
    }

    private static void BlendImages(Mat background, Mat foreground)
    {
        // Split channels using Android OpenCV SDK
        Mat[] channels = new Mat[4];
        OpenCV.Core.Core.Split(foreground, channels);
        Mat alpha = channels[3];
        Mat maskInv = new Mat();
        OpenCV.Core.Core.Bitwise_not(alpha, maskInv);
        Mat backgroundRegion = new Mat();
        OpenCV.Core.Core.Bitwise_and(background, background, backgroundRegion, maskInv);
        Mat foregroundRegion = new Mat();
        OpenCV.Core.Core.Bitwise_and(foreground, foreground, foregroundRegion, alpha);
        OpenCV.Core.Core.Add(backgroundRegion, foregroundRegion, background);
    }
#endif

    [GeneratedRegex("^data:image/[^;]+;base64,")]
    private static partial Regex ImageDataUrlBase64PrefixRegex();
}
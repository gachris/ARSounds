#if WINDOWS
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
#else
using OpenCV.Core;
using OpenCV.ImgProc;
#endif
using OpenVision.Core.DataTypes;
using OpenVision.Core.Utils;
using System.Text.RegularExpressions;
using PointF = System.Drawing.PointF;
using Size = System.Drawing.Size;

namespace ARSounds.UI.Maui.Camera.ViewModels;

internal partial class ARCameraHelper
{
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

#if WINDOWS
        int fullWidth = waveformImage.Width;
        // On Windows we work with Emgu's Image<Bgra, byte>
        using var fullWaveImage = waveformImage.ToImage<Bgra, byte>();
        var (sliceImage, sliceLeft) = CropWaveformSlice(fullWaveImage, audioProgress);
        ReplaceBlackPixels(sliceImage, hexColor);
        var dstCorners = CalculateDestinationCorners(targetMatchResult, sliceLeft, sliceImage.Width, fullWidth);
        if (dstCorners == null) return;
        var srcCorners = GetSourceCorners(sliceImage);
        using var transformedSlice = ApplyPerspectiveTransform(sliceImage, srcCorners, dstCorners, cameraFrame.Size);
        BlendImages(cameraFrame, transformedSlice);
#else
        int fullWidth = waveformImage.Width();
        // On non-Windows, we work with Mat directly (using OpenCvSharp)
        var (sliceMat, sliceLeft) = CropWaveformSlice(waveformImage, audioProgress);
        ReplaceBlackPixels(sliceMat, hexColor);
        var dstCorners = CalculateDestinationCorners(targetMatchResult, sliceLeft, sliceMat.Width(), fullWidth);
        if (dstCorners == null) return;
        var srcCorners = GetSourceCorners(sliceMat);
        using var transformedSlice = ApplyPerspectiveTransform(sliceMat, srcCorners, dstCorners, new Size((int)cameraFrame.Size().Width, (int)cameraFrame.Size().Height));
        BlendImages(cameraFrame, transformedSlice);
#endif
    }

    #region Ensure CameraFrame Has Alpha
    private static void EnsureCameraFrameHasAlpha(Mat cameraFrame)
    {
#if WINDOWS
        if (cameraFrame.NumberOfChannels != 4)
        {
            CvInvoke.CvtColor(cameraFrame, cameraFrame, ColorConversion.Bgr2Bgra);
        }
#else
        if (cameraFrame.Channels() != 4)
        {
            Imgproc.CvtColor(cameraFrame, cameraFrame, Imgproc.ColorBgr2bgra);
        }
#endif
    }
    #endregion

    #region CropWaveformSlice
#if WINDOWS
    private static (Image<Bgra, byte> sliceImage, int sliceLeft) CropWaveformSlice(Image<Bgra, byte> fullWaveImage, double audioProgress)
    {
        int fullWidth = fullWaveImage.Width;
        int fullHeight = fullWaveImage.Height;
        const int sliceWidth = 20;
        int sliceLeft = (int)(audioProgress * (fullWidth - sliceWidth));
        sliceLeft = Math.Max(0, Math.Min(sliceLeft, fullWidth - sliceWidth));
        var cropRect = new System.Drawing.Rectangle(sliceLeft, 0, sliceWidth, fullHeight);
        var sliceImage = fullWaveImage.Copy(cropRect);
        return (sliceImage, sliceLeft);
    }
#else
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
#endif
    #endregion

    #region ReplaceBlackPixels
#if WINDOWS
    private static void ReplaceBlackPixels(Image<Bgra, byte> image, string hexColor)
    {
        var color = System.Drawing.ColorTranslator.FromHtml(hexColor);
        for (int y = 0; y < image.Height; y++)
        {
            for (int x = 0; x < image.Width; x++)
            {
                Bgra pixel = image[y, x];
                // Replace pixels that are exactly black (and not fully transparent)
                if (pixel.Blue == 0 && pixel.Green == 0 && pixel.Red == 0 && pixel.Alpha > 0)
                {
                    image[y, x] = new Bgra(color.B, color.G, color.R, pixel.Alpha);
                }
            }
        }
    }
#else
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
#endif
    #endregion

    #region Destination & Source Corners
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

#if WINDOWS
    private static PointF[] GetSourceCorners(Image<Bgra, byte> sliceImage)
    {
        return new PointF[]
        {
            new PointF(0,0),
            new PointF(sliceImage.Width - 1, 0),
            new PointF(sliceImage.Width - 1, sliceImage.Height - 1),
            new PointF(0, sliceImage.Height - 1)
        };
    }
#else
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
#endif

    /// <summary>
    /// Interpolates between two points based on the given fraction [0..1].
    /// </summary>
    private static PointF InterpolatePoint(PointF start, PointF end, double fraction)
    {
        float x = (float)(start.X + (end.X - start.X) * fraction);
        float y = (float)(start.Y + (end.Y - start.Y) * fraction);
        return new PointF(x, y);
    }
    #endregion

    #region Perspective Transform
#if WINDOWS
    private static Mat ApplyPerspectiveTransform(Image<Bgra, byte> sliceImage, PointF[] srcCorners, PointF[] dstCorners, Size targetSize)
    {
        var transformedSlice = new Mat(targetSize, DepthType.Cv8U, 4);
        transformedSlice.SetTo(new MCvScalar(0, 0, 0, 0));
        using var matrix = CvInvoke.GetPerspectiveTransform(srcCorners, dstCorners);
        CvInvoke.WarpPerspective(sliceImage, transformedSlice, matrix, targetSize, Inter.Linear, Warp.Default, BorderType.Constant);
        return transformedSlice;
    }
#else
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
#endif
    #endregion

    #region BlendImages
#if WINDOWS
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
    #endregion

    [GeneratedRegex("^data:image/[^;]+;base64,")]
    private static partial Regex ImageDataUrlBase64PrefixRegex();
}

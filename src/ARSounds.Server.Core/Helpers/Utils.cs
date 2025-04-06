using System.Text;
using System.Text.RegularExpressions;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ARSounds.Server.Core.Helpers;

public static class Utils
{
    public static string GetOpenVisionImage(string image)
    {
        var imageBytes = image.Base64ToByteArray();

        var matImage = new Mat();
        CvInvoke.Imdecode(imageBytes, ImreadModes.Unchanged, matImage);

        var matModelImage = new Mat(matImage.Size, DepthType.Cv8U, 3);
        matModelImage.SetTo(new MCvScalar(255, 255, 255));

        var alpha = new Mat();
        CvInvoke.ExtractChannel(matImage, alpha, 3);

        var mask = new Mat();
        CvInvoke.Threshold(alpha, mask, 0, 255, ThresholdType.Binary);

        matModelImage.SetTo(new MCvScalar(0, 0, 0), mask);

        var bufferPng = CvInvoke.Imencode(".png", matModelImage);
        return Convert.ToBase64String(bufferPng);
    }

    public static byte[] GetARSoundsImage(string image)
    {
        var buffer = image.Base64ToByteArray();

        var matJpeg = new Mat();
        CvInvoke.Imdecode(buffer, ImreadModes.Color, matJpeg);

        var lowerWhite = new ScalarArray(new MCvScalar(240, 240, 240));
        var upperWhite = new ScalarArray(new MCvScalar(255, 255, 255));
        var whiteMask = new Mat();
        CvInvoke.InRange(matJpeg, lowerWhite, upperWhite, whiteMask);

        var alphaMask = new Mat();
        CvInvoke.BitwiseNot(whiteMask, alphaMask);

        var matWithAlpha = new Mat();
        CvInvoke.CvtColor(matJpeg, matWithAlpha, ColorConversion.Bgr2Bgra);

        CvInvoke.InsertChannel(alphaMask, matWithAlpha, 3);

        return CvInvoke.Imencode(".png", matWithAlpha);
    }

    public static string Base64Encode(this string text)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static byte[] Base64ToByteArray(this string base64)
    {
        ArgumentException.ThrowIfNullOrEmpty(base64);

        // Remove any data URI header if present.
        base64 = Regex.Replace(base64, @"^data:[^;]+;base64,", string.Empty);

        return Convert.FromBase64String(base64);
    }

    public static string GetAsBase64(this byte[] buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        var base64String = Convert.ToBase64String(buffer);
        var mimeType = GetMimeType(buffer);
        var prefix = $"data:{mimeType};base64,";

        // Use a regex to check if the base64 string already starts with a data URI prefix.
        if (!Regex.IsMatch(base64String, @"^data:[^;]+;base64,"))
        {
            base64String = string.Concat(prefix, base64String);
        }

        return base64String;
    }

    private static string GetMimeType(byte[] buffer)
    {
        // Check for JPEG magic numbers.
        if (buffer.Length >= 3 &&
            buffer[0] == 0xFF &&
            buffer[1] == 0xD8 &&
            buffer[2] == 0xFF)
        {
            return "image/jpeg";
        }

        // Check for PNG magic numbers.
        if (buffer.Length >= 8 &&
            buffer[0] == 0x89 &&
            buffer[1] == 0x50 &&
            buffer[2] == 0x4E &&
            buffer[3] == 0x47 &&
            buffer[4] == 0x0D &&
            buffer[5] == 0x0A &&
            buffer[6] == 0x1A &&
            buffer[7] == 0x0A)
        {
            return "image/png";
        }

        // Check for GIF magic numbers.
        if (buffer.Length >= 3 &&
            buffer[0] == 0x47 &&
            buffer[1] == 0x49 &&
            buffer[2] == 0x46)
        {
            return "image/gif";
        }

        // Check for MP3 using the "ID3" tag.
        if (buffer.Length >= 3 &&
            buffer[0] == 0x49 && // 'I'
            buffer[1] == 0x44 && // 'D'
            buffer[2] == 0x33)   // '3'
        {
            return "audio/mpeg";
        }

        // Check for MP3 frame sync if no ID3 tag is present.
        // MP3 frames usually start with 0xFF followed by a byte with its top 3 bits set.
        if (buffer.Length >= 2 &&
            buffer[0] == 0xFF &&
            (buffer[1] & 0xE0) == 0xE0)
        {
            return "audio/mpeg";
        }

        // Check for WAV format: "RIFF" at beginning and "WAVE" at offset 8.
        if (buffer.Length >= 12 &&
            buffer[0] == 0x52 && // 'R'
            buffer[1] == 0x49 && // 'I'
            buffer[2] == 0x46 && // 'F'
            buffer[3] == 0x46 && // 'F'
            buffer[8] == 0x57 && // 'W'
            buffer[9] == 0x41 && // 'A'
            buffer[10] == 0x56 && // 'V'
            buffer[11] == 0x45)   // 'E'
        {
            return "audio/wav";
        }

        // Check for OGG format: "OggS" at the beginning.
        if (buffer.Length >= 4 &&
            buffer[0] == 0x4F && // 'O'
            buffer[1] == 0x67 && // 'g'
            buffer[2] == 0x67 && // 'g'
            buffer[3] == 0x53)   // 'S'
        {
            return "audio/ogg";
        }

        // Fallback MIME type if no known signature is detected.
        return "application/octet-stream";
    }
}
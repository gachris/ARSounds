using System.Text;

namespace ARSounds.Server.Core.Utils;

public static class UtilsExtensions
{
    public static string Base64Encode(this string text)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static byte[] Base64IByteArray(this string base64, string type)
    {
        ArgumentException.ThrowIfNullOrEmpty(base64);

        var prefix = $"data:{type};base64,";
        if (!string.IsNullOrEmpty(prefix) && base64.Contains(prefix)) base64 = base64.Replace(prefix, string.Empty);
        return Convert.FromBase64String(base64);
    }

    public static string GetAsBase64(this byte[] buffer, string type)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        var base64String = Convert.ToBase64String(buffer);
        var prefixBase64 = $"data:{type};base64,";

        if (!string.IsNullOrEmpty(prefixBase64) && !base64String.Contains(prefixBase64))
            base64String = string.Concat(prefixBase64, base64String);
        else if (!string.IsNullOrEmpty(prefixBase64) && base64String.Contains(prefixBase64))
            base64String = base64String.Replace(prefixBase64, string.Empty);
        return base64String;
    }
}
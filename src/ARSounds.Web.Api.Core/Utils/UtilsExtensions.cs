using ARSounds.Web.Api.Core.Attributes;
using ARSounds.Web.Api.Core.Enums;
using System.ComponentModel;
using System.Text;

namespace ARSounds.Web.Api.Core.Utils;

public static class UtilsExtensions
{
    /// <summary>
    /// Gets an attribute on an enum field value
    /// </summary>
    /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
    /// <param name="value">The enum value</param>
    /// <returns>The attribute of type T that exists on the enum value</returns>
    /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
    public static T? GetAttributeOfType<T>(this Enum value) where T : Attribute
    {
        if (value != null)
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T)attributes[0] : null;
        }
        else return null;
    }

    public static string Base64Decode(this string text)
    {
        var plainTextBytes = Convert.FromBase64String(text);
        return Encoding.UTF8.GetString(plainTextBytes);
    }

    public static string Base64Encode(this string text)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(text);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static AudioType? ToAudioType(this string audioType)
    {
        return !string.IsNullOrEmpty(audioType)
            ? Enum.GetValues(typeof(AudioType)).Cast<AudioType>().
                Where(x => x.GetAttributeOfType<DescriptionAttribute>()?.Description == audioType)?.FirstOrDefault() ?? null
            : null;
    }

    public static byte[] Base64ImgToByteArray(this string img, ImagetType imageType)
    {
        if (string.IsNullOrEmpty(img)) throw new ArgumentNullException(nameof(img));

        var prefix = imageType.GetAttributeOfType<Base64PrefixAttribute>()?.Description;
        if (!string.IsNullOrEmpty(prefix) && img.Contains(prefix)) img = img.Replace(prefix, string.Empty);
        return Convert.FromBase64String(img);
    }

    public static string GetAudioAsBase64(this byte[] audio, AudioType audioType, bool usePrefix)
    {
        if (audio == null) throw new ArgumentNullException(nameof(audio));

        var base64Audio = Convert.ToBase64String(audio);
        var prefixAudio = audioType.GetAttributeOfType<Base64PrefixAttribute>()?.Description;

        if (usePrefix && !string.IsNullOrEmpty(prefixAudio) && !base64Audio.Contains(prefixAudio))
            base64Audio = string.Concat(prefixAudio, base64Audio);
        else if (!string.IsNullOrEmpty(prefixAudio) && base64Audio.Contains(prefixAudio))
            base64Audio = base64Audio.Replace(prefixAudio, string.Empty);
        return base64Audio;
    }

    public static string GetImageAsBase64(this byte[] img, ImagetType imageType, bool usePrefix)
    {
        if (img == null) throw new ArgumentNullException(nameof(img));

        var base64Img = Convert.ToBase64String(img);
        var prefixImg = imageType.GetAttributeOfType<Base64PrefixAttribute>()?.Description;

        if (usePrefix && !string.IsNullOrEmpty(prefixImg) && !base64Img.Contains(prefixImg))
            base64Img = string.Concat(prefixImg, base64Img);
        else if (!string.IsNullOrEmpty(prefixImg) && base64Img.Contains(prefixImg))
            base64Img = base64Img.Replace(prefixImg, string.Empty);
        return base64Img;
    }
}
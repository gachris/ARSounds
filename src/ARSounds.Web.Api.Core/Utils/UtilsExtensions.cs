using ARSounds.Web.Api.Core.Attributes;
using ARSounds.Web.Api.Core.Enums;
using System.ComponentModel;
using System.Drawing;
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
    public static T GetAttributeOfType<T>(this Enum value) where T : Attribute
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

    public static bool IsNotNullOrEmpty(this string text)
    {
        return !string.IsNullOrEmpty(text);
    }

    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
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
        return audioType.IsNotNullOrEmpty()
            ? Enum.GetValues(typeof(AudioType)).Cast<AudioType>().
                Where(x => x.GetAttributeOfType<DescriptionAttribute>().Description == audioType)?.FirstOrDefault() ?? null
            : null;
    }

    public static byte[] Base64ImgToByteArray(this string img, ImagetType imagetType)
    {
        if (img.IsNullOrEmpty()) throw new ArgumentNullException(nameof(img));

        string prefix = imagetType.GetAttributeOfType<Base64PrefixAttribute>()?.Description;
        if (img.Contains(prefix)) img = img.Replace(prefix, string.Empty);
        return Convert.FromBase64String(img);
    }

    public static string GetAudioAsBase64(this byte[] audio, AudioType audioType, bool usePrefix)
    {
        if (audio == null) throw new ArgumentNullException(nameof(audio));

        var base64Audio = Convert.ToBase64String(audio);
        var prefixAudio = audioType.GetAttributeOfType<Base64PrefixAttribute>()?.Description;

        if (usePrefix && !base64Audio.Contains(prefixAudio))
            base64Audio = string.Concat(prefixAudio, base64Audio);
        else if (prefixAudio.IsNotNullOrEmpty() && base64Audio.Contains(prefixAudio))
            base64Audio = base64Audio.Replace(prefixAudio, string.Empty);
        return base64Audio;
    }

    public static string GetImageAsBase64(this byte[] img, ImagetType imagetType, bool usePrefix)
    {
        if (img == null) throw new ArgumentNullException(nameof(img));

        var base64Img = Convert.ToBase64String(img);
        var prefixImg = imagetType.GetAttributeOfType<Base64PrefixAttribute>()?.Description;

        if (usePrefix && !base64Img.Contains(prefixImg))
            base64Img = string.Concat(prefixImg, base64Img);
        else if (prefixImg.IsNotNullOrEmpty() && base64Img.Contains(prefixImg))
            base64Img = base64Img.Replace(prefixImg, string.Empty);
        return base64Img;
    }
}
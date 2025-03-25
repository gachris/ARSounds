using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;

namespace ARSounds.UI.Controls.Videos;

public class VideoSourceConverter : TypeConverter, IExtendedTypeConverter
{
    object IExtendedTypeConverter.ConvertFromInvariantString(string value, IServiceProvider serviceProvider)
    {
        return !string.IsNullOrWhiteSpace(value)
            ? (object)(Uri.TryCreate(value, UriKind.Absolute, out Uri uri) && uri.Scheme != "file" ?
                VideoSource.FromUri(value) : VideoSource.FromResource(value))
            : throw new InvalidOperationException("Cannot convert null or whitespace to VideoSource.");
    }
}

using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Controls.Videos;

public class FileVideoSource : VideoSource
{
    public static readonly BindableProperty FileProperty =
        BindableProperty.Create(nameof(File), typeof(string), typeof(FileVideoSource));

    public string File
    {
        get { return (string)GetValue(FileProperty); }
        set { SetValue(FileProperty, value); }
    }
}

namespace ARSounds.UI.Maui.Controls;

public partial class RoundedCornerImage : Border
{
    public static BindableProperty SourceProperty =
            BindableProperty.Create(
                nameof(Source),
                typeof(ImageSource),
                typeof(RoundedCornerImage),
                defaultValue: null
            );

    public ImageSource Source
    {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public RoundedCornerImage()
    {
        InitializeComponent();
    }
}
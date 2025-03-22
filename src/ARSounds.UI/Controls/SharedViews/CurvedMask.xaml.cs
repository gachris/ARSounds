using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ARSounds.MauiApp.Controls;

public partial class CurvedMask : ContentView
{
    public static BindableProperty MaskColorProperty =
            BindableProperty.Create(
                nameof(MaskColor),
                typeof(Color),
                typeof(CurvedMask),
                defaultValue: Color.FromArgb("#ffffff"),
                propertyChanged: OnMaskColorChanged
            );

    public Color MaskColor
    {
        get { return (Color)GetValue(MaskColorProperty); }
        set { SetValue(MaskColorProperty, value); }
    }

    public CurvedMask()
    {
        InitializeComponent();
    }

    private static void OnMaskColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((CurvedMask)bindable).Update();
    }

    private void Update()
    {
        root.Children.Clear();

        if (MaskColor != Color.FromArgb("#ffffff"))
        {
            var image = new Image
            {
                Style = Resources["MaskImageStyle"] as Style
            };

            //image.Transformations.Add(new TintTransformation(MaskColor.ToHex()) { EnableSolidColor = true });
            root.Children.Add(image);
        }
    }
}
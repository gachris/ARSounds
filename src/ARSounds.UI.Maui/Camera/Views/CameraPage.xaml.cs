using ARSounds.UI.Maui.Camera.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Camera.Views;

public partial class CameraPage : ContentPage
{
    public CameraPage(CameraViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
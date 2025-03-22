using ARSounds.UI.Camera.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Camera.Views;

public partial class CameraPage : ContentPage
{
    public CameraPage(CameraViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
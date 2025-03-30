using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class ARCameraPage : ContentPage
{
    public ARCameraPage(ARCameraViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
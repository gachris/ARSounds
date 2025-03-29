using ARSounds.UI.Maui.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class BackgroundPage : ContentPage
{
    public BackgroundPage(BackgroundViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
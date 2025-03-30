using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Maui.Views;

public partial class WalkthroughPage : ContentPage
{
    public WalkthroughPage(WalkthroughViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
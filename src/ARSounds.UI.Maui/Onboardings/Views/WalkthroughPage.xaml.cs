using ARSounds.UI.Maui.Onboardings.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Onboardings.Views;

public partial class WalkthroughPage : ContentPage
{
    public WalkthroughPage(WalkthroughViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
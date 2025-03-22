using ARSounds.UI.Onboardings.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Onboardings.Views;

public partial class WalkthroughPage : ContentPage
{
    public WalkthroughPage(WalkthroughViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
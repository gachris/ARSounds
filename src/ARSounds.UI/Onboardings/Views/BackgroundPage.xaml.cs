using ARSounds.UI.Onboardings.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Onboardings.Views;

public partial class BackgroundPage : ContentPage
{
    public BackgroundPage(BackgroundViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
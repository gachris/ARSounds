using ARSounds.UI.Maui.Onboardings.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Onboardings.Views;

public partial class BackgroundPage : ContentPage
{
    public BackgroundPage(BackgroundViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
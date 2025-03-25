using ARSounds.UI.Maui.User.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.User.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}

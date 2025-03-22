using ARSounds.UI.User.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.User.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}

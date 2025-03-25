using ARSounds.UI.Maui.Camera.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Maui.Camera.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
using ARSounds.UI.Camera.ViewModels;
using Microsoft.Maui.Controls;

namespace ARSounds.UI.Camera.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel bindingContext)
    {
        InitializeComponent();
        BindingContext = bindingContext;
    }
}
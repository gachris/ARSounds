using Microsoft.UI.Xaml.Controls;

namespace ARSounds.UI.WinUI.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        DataContext = ViewModelLocator.SettingsViewModel;
        InitializeComponent();
    }
}

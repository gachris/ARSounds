using System.Windows.Controls;
using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Wpf.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
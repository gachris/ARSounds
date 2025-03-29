using ARSounds.UI.Wpf.ViewModels;
using System.Windows.Controls;

namespace ARSounds.UI.Wpf.Views;

public partial class ARCameraPage : Page
{
    public ARCameraPage(ARCameraViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
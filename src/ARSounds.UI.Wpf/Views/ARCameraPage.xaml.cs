using System.Windows.Controls;
using ARSounds.UI.Common.ViewModels;

namespace ARSounds.UI.Wpf.Views;

public partial class ARCameraPage : Page
{
    public ARCameraPage(ARCameraViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
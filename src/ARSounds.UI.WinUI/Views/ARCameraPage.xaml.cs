using Microsoft.UI.Xaml.Controls;

namespace ARSounds.UI.WinUI.Views;

public sealed partial class ARCameraPage : Page
{
    public ARCameraPage()
    {
        DataContext = ViewModelLocator.ARCameraViewModel;
        InitializeComponent();
    }
}
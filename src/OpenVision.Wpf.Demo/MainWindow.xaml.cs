using System.Windows;
using DevToolbox.Wpf.Windows;

namespace OpenVision.Wpf.Demo;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Image_Reco_Button_Click(object sender, RoutedEventArgs e)
    {
        new ARScene.MainWindow().ShowDialog();
    }

    private void Image_Classification_Button_Click(object sender, RoutedEventArgs e)
    {
        new ML.MainWindow().ShowDialog();
    }
}
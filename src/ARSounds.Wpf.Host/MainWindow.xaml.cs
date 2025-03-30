using ARSounds.UI.Wpf.Views;
using CommonServiceLocator;
using DevToolbox.Wpf.Windows;

namespace ARSounds.Wpf.Host;

public partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        Content = ServiceLocator.Current.GetInstance<AppShellView>();
        InitializeComponent();
    }
}
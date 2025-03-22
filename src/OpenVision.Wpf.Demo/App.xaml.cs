using System.Windows;
using DevToolbox.Wpf.Media;
using OpenVision.Core.Configuration;

namespace OpenVision.Wpf.Demo;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        VisionSystemConfig.WebSocketUrl = "wss://localhost:44320/ws";

        base.OnStartup(e);

        ThemeManager.RequestedTheme = ElementTheme.WindowsDefault;
    }
}
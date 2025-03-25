using System.Windows;
using DevToolbox.Wpf.Media;
using OpenVision.Core.Configuration;

namespace ARSounds.Wpf.Host;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public App()
    {
        IocConfiguration.Setup();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        VisionSystemConfig.WebSocketUrl = "wss://localhost:44320/ws";

        base.OnStartup(e);

        ThemeManager.RequestedTheme = ElementTheme.WindowsDefault;
    }
}
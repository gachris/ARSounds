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
        VisionSystemConfig.ImageRequestBuilder = new OpenVision.Core.DataTypes.ImageRequestBuilder()
            .WithGrayscale()
            .WithGaussianBlur(new System.Drawing.Size(5, 5), 0)
            .WithLowResolution(320);

        VisionSystemConfig.WebSocketUrl = "wss://localhost:44320/ws";
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        IocConfiguration.Setup();

        ThemeManager.RequestedTheme = ElementTheme.WindowsDefault;
    }
}
using Microsoft.Extensions.Logging;
using OpenVision.Core.Configuration;
using OpenVision.Maui.Controls;

namespace OpenVision.Maui.Demo;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        VisionSystemConfig.WebSocketUrl = "wss://localhost:44320/ws";

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(handlers => handlers.AddHandler(typeof(OpenVision.Maui.Controls.ARCamera), typeof(ARCameraHandler)))
            .ConfigureMauiHandlers(handlers => handlers.AddHandler(typeof(Camera), typeof(CameraHandler)));

#if DEBUG
		builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}

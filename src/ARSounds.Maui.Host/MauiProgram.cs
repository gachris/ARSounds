using ARSounds.Application;
using ARSounds.Application.Services;
using ARSounds.Core;
using ARSounds.Core.Configuration;
using ARSounds.Maui.Host.Helpers;
using ARSounds.UI.Common;
using ARSounds.UI.Maui;
using CommonServiceLocator;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using OpenVision.Maui.Controls;

namespace ARSounds.Maui.Host;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCompatibility()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler<ARCamera, ARCameraHandler>();
            });

#if WINDOWS
        builder.Configuration
            .AddJsonFromPackageFile("appsettings.windows.json");
#else
        builder.Configuration
            .AddJsonFromPackageFile("appsettings.android.json");
#endif

        if (SynchronizationContext.Current is not null)
            builder.Services.AddSingleton(SynchronizationContext.Current);

        var appConfiguration = builder.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>()!;

        builder.Services.AddSingleton(appConfiguration);

        var appData = FileSystem.Current.AppDataDirectory;
        var folderPath =  Path.Combine(appData, appConfiguration.ApplicationName);
        builder.Services.AddSingleton<IDataStore, FileDataStore>(t => new FileDataStore(folderPath, true));

        builder.Services.AddSingleton(Connectivity.Current);
        builder.Services.AddSingleton(t => ServiceLocator.Current);

        builder.Services.AddLocalization();

        builder.Services.ConfigureOpenVision(appConfiguration.OpenVisionWebSocketUrl);

        builder.Services.AddCore();
        builder.Services.AddApplication();
        builder.Services.AddUI();

        var mauiApp = builder.Build();

        ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(mauiApp.Services));

        return mauiApp;
    }
}

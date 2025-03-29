using ARSounds.Application;
using ARSounds.Application.Services;
using ARSounds.Core;
using ARSounds.Core.Configuration;
using ARSounds.UI.Maui;
using CommonServiceLocator;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using FileDataStore = ARSounds.UI.Maui.Services.FileDataStore;

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
            .ConfigureFonts(fonts => fonts.AddFonts())
            .ConfigureMauiHandlers(handlers => handlers.AddHandlers());

#if WINDOWS
        builder.Configuration
            .AddJsonFromPackageFile("appsettings.windows.json");
#else
        builder.Configuration
            .AddJsonFromPackageFile("appsettings.android.json");
#endif

        var appConfiguration = builder.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>()!;

        builder.Services.AddSingleton(appConfiguration);

        builder.Services.AddSingleton<IDataStore, FileDataStore>(t => new FileDataStore(appConfiguration.ApplicationName));

        builder.Services.AddSingleton(Connectivity.Current);
        builder.Services.AddSingleton(t => ServiceLocator.Current);

        builder.Services.AddLocalization();
        builder.Services.AddSynchronizationContext();

        builder.Services.AddCore();
        builder.Services.AddApplication();
        builder.Services.AddUI();

        var mauiApp = builder.Build();

        ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(mauiApp.Services));

        return mauiApp;
    }
}

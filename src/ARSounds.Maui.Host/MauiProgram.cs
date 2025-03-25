using ARSounds.Application;
using ARSounds.Core;
using ARSounds.Core.Configuration;
using ARSounds.UI;
using CommonServiceLocator;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Compatibility.Hosting;

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

        builder.Configuration
            .AddJsonFromPackageFile("appsettings.json");

        var appConfiguration = builder.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
        var oidcConfiguration = builder.Configuration.GetRequiredSection(nameof(OidcConfiguration)).Get<OidcConfiguration>();

        builder.Services.AddSingleton(appConfiguration);
        builder.Services.AddSingleton(oidcConfiguration);
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

using ARSounds.Application;
using ARSounds.Application.Configuration;
using ARSounds.Core;
using ARSounds.UI.Common;
using ARSounds.UI.Maui;
using ARSounds.UI.Maui.Browser;
using CommonServiceLocator;
using CommunityToolkit.Maui;
using DevToolbox.Core;
using DevToolbox.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using OpenVision.Maui.Controls;

namespace ARSounds.Maui.Host;

public static class MauiProgram
{
    #region Methods

    public static MauiApp CreateMauiApp()
    {
        try
        {
            var builder = MauiApp.CreateBuilder();

            builder.Configuration.AddAppSettings();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiCompatibility()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddMaterialIcons();

                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler<ARCamera, ARCameraHandler>();
                })
                .ConfigureServices(services =>
                {
                    var webAuthenticatorBrowser = new WebAuthenticatorBrowser();
                    var localSettingsOptions = builder.Configuration.GetRequiredSection(nameof(LocalSettingsOptions)).Get<LocalSettingsOptions>();
                    var appConfiguration = builder.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
                    ArgumentNullException.ThrowIfNull(appConfiguration, nameof(appConfiguration));

                    var folderPath = Path.Combine(FileSystem.Current.AppDataDirectory, appConfiguration.ApplicationName);

                    if (localSettingsOptions is not null)
                    {
                        services.AddSingleton(t => localSettingsOptions);
                    }

                    services.AddSingleton(appConfiguration);
                    services.AddSingleton(t => ServiceLocator.Current);
                    services.AddSynchronizationContext();
                    services.AddCore();
                    services.AddApplication();
                    services.AddUI();
                    services.AddClient(
                        appConfiguration,
                        webAuthenticatorBrowser,
                        folderPath,
                        true);
                });

            var mauiApp = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(mauiApp.Services));

            return mauiApp;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up the IoC: {ex.Message}");
            throw;
        }
    }

    private static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configuration)
    {
        var fileName = "appsettings";

        configuration
            .AddJsonStreamPackageFile($"{fileName}.json");
#if WINDOWS
        configuration
            .AddJsonStreamPackageFile($"{fileName}.windows.json");
#elif ANDROID
        configuration
            .AddJsonStreamPackageFile($"{fileName}.android.json");
#endif

        return configuration;
    }

    #endregion
}

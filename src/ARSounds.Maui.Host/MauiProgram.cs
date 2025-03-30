using ARSounds.Application;
using ARSounds.Application.Configuration;
using ARSounds.Core;
using ARSounds.UI.Common;
using ARSounds.UI.Common.Services;
using ARSounds.UI.Maui;
using ARSounds.UI.Maui.Browser;
using CommonServiceLocator;
using CommunityToolkit.Maui;
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
                    services.Configure<LocalSettingsOptions>(builder.Configuration.GetSection(nameof(LocalSettingsOptions)));

                    var appConfiguration = builder.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
                    ArgumentNullException.ThrowIfNull(appConfiguration, nameof(appConfiguration));

                    var folderPath = Path.Combine(FileSystem.Current.AppDataDirectory, appConfiguration.ApplicationName);

                    services.AddSingleton(appConfiguration);
                    services.AddSingleton(t => ServiceLocator.Current);
                    services.AddClientOptions(folderPath, true, new WebAuthenticatorBrowser(), appConfiguration);
                    services.ConfigureOpenVision(appConfiguration.OpenVisionWebSocketUrl);

                    if (SynchronizationContext.Current is not null)
                    {
                        services.AddSingleton(t => SynchronizationContext.Current);
                    }

                    services.AddCore();
                    services.AddApplication();
                    services.AddUI();
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

    private static IConfigurationBuilder AddJsonStreamPackageFile(this IConfigurationBuilder configuration, string fileName)
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(fileName).ConfigureAwait(false).GetAwaiter().GetResult();
        return configuration.AddJsonStream(stream);
    }

    private static MauiAppBuilder ConfigureServices(this MauiAppBuilder mauiAppBuilder, Action<IServiceCollection> configureDelegate)
    {
        configureDelegate.Invoke(mauiAppBuilder.Services);
        return mauiAppBuilder;
    }

    #endregion
}

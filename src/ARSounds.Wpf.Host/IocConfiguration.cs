using ARSounds.Application;
using ARSounds.Application.Configuration;
using ARSounds.Core;
using ARSounds.UI.Common;
using ARSounds.UI.Common.Services;
using ARSounds.UI.Wpf;
using ARSounds.UI.Wpf.Browser;
using CommonServiceLocator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ARSounds.Wpf.Host;

public static class IocConfiguration
{
    #region Properties

    public static IHost? AppHost { get; private set; }

    #endregion

    #region Methods

    public static void Setup()
    {
        try
        {
            var builder = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder();

            builder
               .UseContentRoot(AppContext.BaseDirectory)
               .ConfigureServices((context, services) =>
               {
                   services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));

                   var appConfiguration = context.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
                   ArgumentNullException.ThrowIfNull(appConfiguration, nameof(appConfiguration));

                   services.AddSingleton(appConfiguration);
                   services.AddSingleton(t => ServiceLocator.Current);
                   services.AddClientOptions(appConfiguration.ApplicationName, false, new WebAuthenticatorBrowser(), appConfiguration);
                   services.ConfigureOpenVision(appConfiguration.OpenVisionWebSocketUrl);

                   if (SynchronizationContext.Current is not null)
                   {
                       services.AddSingleton(t => SynchronizationContext.Current);
                   }

                   services.AddCore();
                   services.AddApplication();
                   services.AddUI();
               })
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.AddConsole();
               });

            AppHost = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(AppHost.Services));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up the IoC: {ex.Message}");
            throw;
        }
    }

    public static async Task ShutdownAsync()
    {
        if (AppHost is null)
        {
            return;
        }

        await AppHost.StopAsync();

        AppHost.Dispose();
        AppHost = null;
    }

    #endregion
}
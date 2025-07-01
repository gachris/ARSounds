using ARSounds.Application;
using ARSounds.Application.Configuration;
using ARSounds.Core;
using ARSounds.UI.Common;
using ARSounds.UI.Wpf;
using ARSounds.UI.Wpf.Browser;
using CommonServiceLocator;
using DevToolbox.Core;
using DevToolbox.Core.Services;
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
                   var webAuthenticatorBrowser = new WebAuthenticatorBrowser();
                   var localSettingsOptions = context.Configuration.GetRequiredSection(nameof(LocalSettingsOptions)).Get<LocalSettingsOptions>();
                   var appConfiguration = context.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
                   ArgumentNullException.ThrowIfNull(appConfiguration, nameof(appConfiguration));

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
                       appConfiguration.ApplicationName,
                       false);
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
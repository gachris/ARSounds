using ARSounds.Application;
using ARSounds.Application.Store;
using ARSounds.Core;
using ARSounds.Core.Configuration;
using ARSounds.UI.WinUI.Store;
using CommonServiceLocator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ARSounds.UI.WinUI;

namespace ARSounds.WinUI.Host;

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
                   var appConfiguration = context.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>()!;
                   var oidcConfiguration = context.Configuration.GetRequiredSection(nameof(OidcConfiguration)).Get<OidcConfiguration>()!;

                   services.AddSingleton(appConfiguration);
                   services.AddSingleton(oidcConfiguration);

                   services.AddSingleton<IDataStore, FileDataStore>(t => new FileDataStore(appConfiguration.ApplicationName));

                   services.AddSingleton(t => ServiceLocator.Current);

                   services.AddSingleton(t => SynchronizationContext.Current);

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

            // Set ServiceLocator provider for legacy use
            ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(AppHost.Services));
        }
        catch (Exception ex)
        {
            // Handle initialization failures (logging, UI message, etc.)
            Console.WriteLine($"Error setting up the IoC: {ex.Message}");
            throw;
        }
    }

    public static async Task ShutdownAsync()
    {
        if (AppHost != null)
        {
            await AppHost.StopAsync();
            AppHost.Dispose();
            AppHost = null;
        }
    }

    #endregion
}
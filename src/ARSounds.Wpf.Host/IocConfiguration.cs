using ARSounds.Application;
using ARSounds.Application.Services;
using ARSounds.Core;
using ARSounds.Core.Configuration;
using ARSounds.UI.Wpf;
using ARSounds.UI.Wpf.Contracts;
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
                   if (SynchronizationContext.Current is not null)
                   {
                       services.AddSingleton(t => SynchronizationContext.Current);
                   }

                   var appConfiguration = context.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
                   ArgumentNullException.ThrowIfNull(appConfiguration, nameof(appConfiguration));

                   services.AddSingleton(appConfiguration);

                   // TODO: must moved to ApplicationModule
                   services.AddSingleton<IDataStore, FileDataStore>(t => new FileDataStore(appConfiguration.ApplicationName));

                   services.AddSingleton<IApplication>(sp => (App)System.Windows.Application.Current);
                   services.AddSingleton(t => ServiceLocator.Current);
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
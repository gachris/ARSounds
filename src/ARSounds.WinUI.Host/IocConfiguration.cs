using ARSounds.Application;
using ARSounds.Core;
using ARSounds.Core.Configuration;
using CommonServiceLocator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ARSounds.UI.WinUI;
using ARSounds.Application.Services;
using ARSounds.UI.WinUI.Services;

namespace ARSounds.WinUI.Host;

public static class IocConfiguration
{
    #region Properties

    public static MainWindow? AppWindow { get; private set; }

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
                       services.AddSingleton(SynchronizationContext.Current);
                   }

                   var appConfiguration = context.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
                   ArgumentNullException.ThrowIfNull(appConfiguration, nameof(appConfiguration));

                   services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));

                   services.AddSingleton(appConfiguration);

                   // TODO: must moved to ApplicationModule
                   services.AddSingleton<IDataStore, FileDataStore>(t => new FileDataStore(appConfiguration.ApplicationName));
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

            // Set ServiceLocator provider for legacy use
            ServiceLocator.SetLocatorProvider(() => new AppServiceLocator(AppHost.Services));

            // Initialize and register the MainWindow
            InitializeMainWindow();
        }
        catch (Exception ex)
        {
            // Handle initialization failures (logging, UI message, etc.)
            Console.WriteLine($"Error setting up the IoC: {ex.Message}");
            throw;
        }
    }

    private static void InitializeMainWindow()
    {
        if (AppWindow != null)
        {
            throw new InvalidOperationException("The MainWindow has already been initialized.");
        }

        AppWindow = new MainWindow();

        // Register the AppWindow in the window service
        var appWindowService = AppHost?.Services.GetService<IAppWindowService>()
            ?? throw new InvalidOperationException("Failed to retrieve IAppWindowService from IoC container.");

        appWindowService.Register(AppWindow);
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
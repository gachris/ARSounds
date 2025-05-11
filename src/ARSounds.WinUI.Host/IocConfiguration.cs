using ARSounds.Application;
using ARSounds.Application.Configuration;
using ARSounds.Core;
using ARSounds.UI.Common;
using ARSounds.UI.WinUI;
using ARSounds.UI.WinUI.Browser;
using CommonServiceLocator;
using DevToolbox.Core;
using DevToolbox.Core.Services;
using DevToolbox.WinUI.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
                   var webAuthenticatorBrowser = new WebAuthenticatorBrowser();
                   var localSettingsOptionsConfiguration = context.Configuration.GetSection(nameof(LocalSettingsOptions));
                   var appConfiguration = context.Configuration.GetRequiredSection(nameof(AppConfiguration)).Get<AppConfiguration>();
                   ArgumentNullException.ThrowIfNull(appConfiguration, nameof(appConfiguration));

                   services.Configure<LocalSettingsOptions>(localSettingsOptionsConfiguration);
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

            InitializeMainWindow();
        }
        catch (Exception ex)
        {
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
using ARSounds.UI.Common.Data;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Wpf.Views;
using DevToolbox.Core;
using DevToolbox.Core.Contracts;
using DevToolbox.Core.Services;
using DevToolbox.Wpf.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.UI.Wpf;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static void AddUI(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
        services.AddIUIServices(typeof(ServiceCollectionExtensions).Assembly);

        // Pages
        services.AddPageService(options =>
        {
            options.Configure(PageKeys.CameraPage, typeof(ARCameraPage));
            options.Configure(PageKeys.SettingsPage, typeof(SettingsPage));
        });

        // Controls
        services.AddSingleton<AppShellView>();
        services.AddSingleton<ARCameraPage>();
        services.AddSingleton<SettingsPage>();

        // ViewModels
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<AccountViewModel>();
        services.AddSingleton<ARCameraViewModel>();
        services.AddSingleton<SettingsViewModel>();

        // AutoMapper
        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }

    #endregion
}
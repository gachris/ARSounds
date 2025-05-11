using ARSounds.UI.Common;
using ARSounds.UI.Common.Data;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.WinUI.Activation;
using ARSounds.UI.WinUI.Services;
using ARSounds.UI.WinUI.Views;
using DevToolbox.Core;
using DevToolbox.Core.Contracts;
using DevToolbox.Core.Services;
using DevToolbox.WinUI.Activation;
using DevToolbox.WinUI.Contracts;
using DevToolbox.WinUI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace ARSounds.UI.WinUI;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static void AddUI(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<IActivationService, ActivationService>();
        services.AddSingleton<IAppWindowService, AppWindowService>();
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

        // ActivationHandler
        services.AddSingleton<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        // Controls
        services.AddSingleton<AppShellPage>();
        services.AddSingleton<SettingsPage>();
        services.AddSingleton<ARCameraPage>();

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
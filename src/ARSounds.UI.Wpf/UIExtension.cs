using ARSounds.UI.Common;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Data;
using ARSounds.UI.Common.Services;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Wpf.Contracts;
using ARSounds.UI.Wpf.Services;
using ARSounds.UI.Wpf.Views;

namespace Microsoft.Extensions.DependencyInjection;

public static class UIExtension
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
        services.AddIUIServices(typeof(UIExtension).Assembly);

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
        services.AddAutoMapper(typeof(UIExtension).Assembly);
    }

    #endregion
}
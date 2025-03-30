using ARSounds.Application.Services;
using ARSounds.UI.Common;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Wpf.Browser;
using ARSounds.UI.Wpf.Contracts;
using ARSounds.UI.Wpf.Services;
using ARSounds.UI.Wpf.ViewModels;
using ARSounds.UI.Wpf.Views;
using IdentityModel.OidcClient.Browser;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.UI.Wpf;

public static class UIModule
{
    #region Methods

    public static void AddUI(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddIUIServices(typeof(UIModule).Assembly);

        // WebAuthenticatorBrowser
        services.AddSingleton<IBrowser, WebAuthenticatorBrowser>();

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
        services.AddAutoMapper(typeof(UIModule).Assembly);
    }

    #endregion
}
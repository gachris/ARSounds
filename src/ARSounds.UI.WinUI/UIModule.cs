using ARSounds.Application.Services;
using ARSounds.UI.Common;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.WinUI.Activation;
using ARSounds.UI.WinUI.Browser;
using ARSounds.UI.WinUI.Contracts;
using ARSounds.UI.WinUI.Services;
using ARSounds.UI.WinUI.ViewModels;
using ARSounds.UI.WinUI.Views;
using IdentityModel.OidcClient.Browser;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;

namespace ARSounds.UI.WinUI;

public static class UIModule
{
    #region Methods

    public static void AddUI(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<IActivationService, ActivationService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<IAppWindowService, AppWindowService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddIUIServices(typeof(UIModule).Assembly);

        // ActivationHandler
        services.AddSingleton<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        // WebAuthenticatorBrowser
        services.AddSingleton<IBrowser, WebAuthenticatorBrowser>();

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
        services.AddAutoMapper(typeof(UIModule).Assembly);
    }

    #endregion
}
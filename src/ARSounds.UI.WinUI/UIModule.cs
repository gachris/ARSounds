using ARSounds.Application.Services;
using ARSounds.UI.Common;
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
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<IActivationService, ActivationService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IAppWindowService, AppWindowService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IBrowser, WebAuthenticatorBrowser>();
        services.AddSingleton<IAuthService, AuthService>();

        services.AddSingleton<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

        services.AddIUIServices(typeof(UIModule).Assembly);

        services.AddSingleton<ShellPage>();
        services.AddSingleton<ShellViewModel>();

        services.AddSingleton<AccountViewModel>();

        services.AddSingleton<ARCameraPage>();
        services.AddSingleton<ARCameraViewModel>();

        services.AddSingleton<SettingsPage>();
        services.AddSingleton<SettingsViewModel>();

        services.AddAutoMapper(typeof(UIModule).Assembly);
    }

    #endregion
}

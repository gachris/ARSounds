using ARSounds.Application.Services;
using ARSounds.UI.Common;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Maui.Contracts;
using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.ViewModels;
using ARSounds.UI.Maui.Views;
using CommunityToolkit.Maui;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace ARSounds.UI.Maui;

public static class UIModule
{
    #region Methods

    public static void AddUI(this IServiceCollection services)
    {
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<INavigationService, NavigationService>();
#if WINDOWS
        services.AddSingleton<IBrowser, WinUI.Browser.WebAuthenticatorBrowser>();
#else
        services.AddSingleton<IBrowser, Platforms.Android.Browser.WebAuthenticatorBrowser>();
#endif
        services.AddSingleton<IAuthService, AuthService>();

        services.AddIUIServices(typeof(UIModule).Assembly);

        services.AddSingleton<AppShell>();
        services.AddSingleton<ShellViewModel>();

        services.AddSingleton<BackgroundPage>();
        services.AddSingleton<BackgroundViewModel>();

        services.AddSingleton<WalkthroughPage>();
        services.AddSingleton<WalkthroughViewModel>();

        services.AddSingleton<AccountViewModel>();
        
        services.AddSingleton<ARCameraPage>();
        services.AddSingleton<ARCameraViewModel>();

        services.AddSingleton<SettingsPage>();
        services.AddSingleton<SettingsViewModel>();

        services.AddAutoMapper(typeof(UIModule).Assembly);
    }

    #endregion
}
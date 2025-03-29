using ARSounds.Application.Services;
using ARSounds.UI.Maui.ViewModels;
using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.Views;
using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;
using ARSounds.UI.Common;

namespace ARSounds.UI.Maui;

public static class UIModule
{
    #region Methods

    public static void AddUI(this IServiceCollection services)
    {
        var navigationMapping = new NavigationMapping();

        navigationMapping.Add<AppShellViewModel, AppShell>();
        navigationMapping.Add<BackgroundViewModel, BackgroundPage>();
        navigationMapping.Add<WalkthroughViewModel, WalkthroughPage>();
        navigationMapping.Add<HomeViewModel, HomePage>();
        navigationMapping.Add<ProfileViewModel, ProfilePage>();
        navigationMapping.Add<SettingsViewModel, SettingsPage>();
        navigationMapping.Add<LoginViewModel, LoginPage>();

#if WINDOWS
        services.AddSingleton<IBrowser, WinUI.Browser.WebAuthenticatorBrowser>();
#else
        services.AddSingleton<IBrowser, Platforms.Android.Browser.WebAuthenticatorBrowser>();
#endif
        services.AddSingleton<IAuthService, AuthService>();

        services.AddSingleton<INavigationService, NavigationService>((provider) => new NavigationService(navigationMapping));

        services.AddIUIServices(typeof(UIModule).Assembly);

        services.AddSingleton<AppShell>();
        services.AddSingleton<AppShellViewModel>();

        services.AddSingleton<BackgroundPage>();
        services.AddSingleton<BackgroundViewModel>();

        services.AddSingleton<WalkthroughPage>();
        services.AddSingleton<WalkthroughViewModel>();

        services.AddTransient<HomePage>();
        services.AddTransient<HomeViewModel>();

        services.AddTransient<ProfilePage>();
        services.AddTransient<ProfileViewModel>();

        services.AddTransient<SettingsPage>();
        services.AddTransient<SettingsViewModel>();

        services.AddTransient<LoginPage>();
        services.AddTransient<LoginViewModel>();

        services.AddSingleton<IAudioManager, AudioManager>();

        services.AddAutoMapper(typeof(UIModule).Assembly);
    }

    #endregion
}

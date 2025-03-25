using ARSounds.Application.Auth;
using ARSounds.UI.Maui.Auth;
using ARSounds.UI.Maui.Camera.ViewModels;
using ARSounds.UI.Maui.Camera.Views;
using ARSounds.UI.Maui.Common.ViewModels;
using ARSounds.UI.Maui.Common.Views;
using ARSounds.UI.Maui.Onboardings.ViewModels;
using ARSounds.UI.Maui.Onboardings.Views;
using ARSounds.UI.Maui.Services;
using ARSounds.UI.Maui.Targets.ViewModels;
using ARSounds.UI.Maui.Targets.Views;
using ARSounds.UI.Maui.User.ViewModels;
using ARSounds.UI.Maui.User.Views;
using CommunityToolkit.Maui;
using Plugin.Maui.Audio;

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
        navigationMapping.Add<CameraViewModel, CameraPage>();
        navigationMapping.Add<TargetsListViewModel, TargetsListPage>();
        navigationMapping.Add<TargetDetailsViewModel, TargetDetailsPage>();
        navigationMapping.Add<User.ViewModels.SettingsViewModel, User.Views.SettingsPage>();
        navigationMapping.Add<LoginViewModel, LoginPage>();
        navigationMapping.Add<Camera.ViewModels.SettingsViewModel, Camera.Views.SettingsPage>();

        services.AddSingleton<IAuthService, AuthService>();

        services.AddSingleton<INavigationService, NavigationService>((provider) => new NavigationService(navigationMapping));

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

        services.AddTransient<CameraPage>();
        services.AddTransient<CameraViewModel>();

        services.AddTransient<TargetsListPage>();
        services.AddTransient<TargetsListViewModel>();

        services.AddTransient<TargetDetailsPage>();
        services.AddTransient<TargetDetailsViewModel>();

        services.AddTransient<User.Views.SettingsPage>();
        services.AddTransient<User.ViewModels.SettingsViewModel>();

        services.AddTransient<Camera.Views.SettingsPage>();
        services.AddTransient<Camera.ViewModels.SettingsViewModel>();

        services.AddTransient<LoginPage>();
        services.AddTransient<LoginViewModel>();

        services.AddSingleton<IdentityModel.OidcClient.Browser.IBrowser, WebAuthenticatorBrowser>();
        services.AddSingleton<IAudioManager, AudioManager>();
    }

    #endregion
}

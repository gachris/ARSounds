using ARSounds.Application.Services;
using ARSounds.UI.Common;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.ViewModels;
using ARSounds.UI.Maui.Browser;
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
        // Services
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<IBrowser, WebAuthenticatorBrowser>();
        services.AddIUIServices(typeof(UIModule).Assembly);

        // WebAuthenticatorBrowser
        services.AddSingleton<INavigationService, NavigationService>();

        // Controls
        services.AddSingleton<AppShellPage>();
        services.AddSingleton<ARCameraPage>();
        services.AddSingleton<SettingsPage>();
        services.AddSingleton<BackgroundPage>();
        services.AddSingleton<WalkthroughPage>();

        // ViewModels
        services.AddSingleton<ShellViewModel>();
        services.AddSingleton<AccountViewModel>();
        services.AddSingleton<ARCameraViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<BackgroundViewModel>();
        services.AddSingleton<WalkthroughViewModel>();

        // AutoMapper
        services.AddAutoMapper(typeof(UIModule).Assembly);
    }

    public static void AddMaterialIcons(this IFontCollection fonts)
    {
        fonts.AddFont("MaterialIcons-Regular.ttf", "MaterialIconsFilled");
        fonts.AddFont("MaterialIconsOutlined-Regular.otf", "MaterialIconsOutlined");
        fonts.AddFont("MaterialIconsRound-Regular.otf", "MaterialIconsRound");
        fonts.AddFont("MaterialIconsSharp-Regular.otf", "MaterialIconsSharp");
        fonts.AddFont("MaterialIconsTwoTone-Regular.otf", "MaterialIconsTwoTone");
    }

    #endregion
}
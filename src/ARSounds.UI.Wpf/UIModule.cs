using ARSounds.Application.Services;
using ARSounds.UI.Common;
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
        services.AddSingleton<IAppUISettings, AppUISettings>();
        services.AddSingleton<IApplicationService, ApplicationService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IBrowser, WebAuthenticatorBrowser>();
        services.AddSingleton<IAuthService, AuthService>();

        services.AddIUIServices(typeof(UIModule).Assembly);

        services.AddSingleton<ShellView>();
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
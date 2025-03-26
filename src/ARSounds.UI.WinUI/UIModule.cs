using ARSounds.Application.Auth;
using ARSounds.UI.WinUI.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.UI.WinUI;

public static class UIModule
{
    #region Methods

    public static void AddUI(this IServiceCollection services)
    {
        services.AddSingleton<IAuthService, AuthService>();
    }

    #endregion
}

using System.Reflection;
using ARSounds.ApiClient.Data;
using ARSounds.ApiClient.DataStore;
using ARSounds.Application.Configuration;
using ARSounds.UI.Common.Contracts;
using ARSounds.UI.Common.Services;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Microsoft.Extensions.DependencyInjection;
using OpenVision.Core.Configuration;

namespace ARSounds.UI.Common;

public static class UIExtensions
{
    #region Methods

    public static IServiceCollection AddSynchronizationContext(this IServiceCollection services)
    {
        if (SynchronizationContext.Current is not null)
        {
            services.AddSingleton(t => SynchronizationContext.Current);
        }

        return services;
    }

    public static IServiceCollection AddPageService(this IServiceCollection services, Action<PageService>? configure = null)
    {
        var pageService = new PageService();

        configure?.Invoke(pageService);

        services.AddSingleton<IPageService>(pageService);

        return services;
    }

    public static void AddClient(this IServiceCollection services, AppConfiguration appConfiguration, IBrowser browser, string cachePath, bool fullPath)
    {
        VisionSystemConfig.WebSocketUrl = appConfiguration.OpenVisionWebSocketUrl;

        VisionSystemConfig.ImageRequestBuilder = new OpenVision.Core.DataTypes.ImageRequestBuilder()
            .WithGrayscale()
            .WithGaussianBlur(new System.Drawing.Size(5, 5), 0)
            .WithLowResolution(320);

        services.AddSingleton<IDataStore, FileDataStore>(t => new FileDataStore(cachePath, fullPath));

        services.Configure<ARSoundsApiOptions>((options) =>
        {
            options.Url = appConfiguration.ARSoundsApiUrl;
        });

        services.Configure<OidcClientOptions>((options) =>
        {
            options.Authority = appConfiguration.Authority;
            options.Scope = appConfiguration.Scope;
            options.ClientId = appConfiguration.ClientId;
            options.RedirectUri = appConfiguration.RedirectUri;
            options.PostLogoutRedirectUri = appConfiguration.PostLogoutRedirectUri;
            options.Browser = browser;
            options.Policy = new Policy
            {
                RequireIdentityTokenSignature = false
            };
        });
    }

    public static void AddIUIServices(this IServiceCollection services, Assembly assembly)
    {
        var uiServiceType = typeof(IUIService);

        var uiServiceInterfaces = assembly.GetTypes()
            .Where(t => t.IsInterface && !t.IsGenericType && t.GetInterfaces().Contains(uiServiceType))
            .ToList();

        var types = assembly.GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();

        foreach (var type in types)
        {
            var implementedInterfaces = type.GetInterfaces()
                .Where(uiServiceInterfaces.Contains)
                .ToList();

            foreach (var interfaceType in implementedInterfaces)
            {
                services.AddSingleton(interfaceType, type);
            }
        }
    }

    #endregion
}

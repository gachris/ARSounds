using ARSounds.ApiClient.Data;
using ARSounds.ApiClient.DataStore;
using ARSounds.Application.Configuration;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Microsoft.Extensions.DependencyInjection;
using OpenVision.Core.Configuration;

namespace ARSounds.UI.Common;

public static class ServiceCollectionExtensions
{
    #region Methods

    public static IServiceCollection AddClient(this IServiceCollection services, AppConfiguration appConfiguration, IBrowser browser, string cachePath, bool fullPath)
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

        return services;
    }

    #endregion
}

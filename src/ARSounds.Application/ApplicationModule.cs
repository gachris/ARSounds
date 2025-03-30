using ARSounds.ApiClient.Contracts;
using ARSounds.ApiClient.Data;
using ARSounds.ApiClient.DataStore;
using ARSounds.ApiClient.Services;
using ARSounds.Application.Configuration;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARSounds.Application;

/// <summary>
/// Provides application-level service registrations for dependency injection.
/// </summary>
public static class ApplicationModule
{
    #region Methods

    /// <summary>
    /// Registers services, pipelines, AutoMapper profiles, and logging
    /// related to the ARSounds application layer.
    /// </summary>
    /// <param name="services">The service collection to add registrations to.</param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR((configuration) => configuration.RegisterServicesFromAssembly(typeof(ApplicationModule).Assembly));

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ITargetsService, TargetsService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
        services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(RequestPostProcessor<,>));

        services.AddLogging((loggingBuilder) =>
        {
            services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
        });

        services.AddAutoMapper(typeof(ApplicationModule).Assembly);
    }

    public static void AddClientOptions(this IServiceCollection services, string folder, bool fullPath, IBrowser browser, AppConfiguration appConfiguration)
    {
        services.AddSingleton<IDataStore, FileDataStore>(t => new FileDataStore(folder, fullPath));

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

    #endregion
}

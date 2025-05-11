using ARSounds.ApiClient.Contracts;
using ARSounds.ApiClient.Services;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARSounds.Application;

/// <summary>
/// Provides application-level service registrations for dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers services, pipelines, AutoMapper profiles, and logging
    /// related to the ARSounds application layer.
    /// </summary>
    /// <param name="services">The service collection to add registrations to.</param>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR((configuration) => configuration.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ITargetsService, TargetsService>();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
        services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(RequestPostProcessor<,>));

        services.AddLogging((loggingBuilder) =>
        {
            services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
        });

        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}

using ARSounds.Application.Services;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ARSounds.Application;

public static class ApplicationModule
{
    #region Methods

    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR((configuration) => configuration.RegisterServicesFromAssembly(typeof(ApplicationModule).Assembly));

        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
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

    #endregion
}

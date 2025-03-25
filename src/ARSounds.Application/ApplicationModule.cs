using ARSounds.Application.ImageRecognition;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.Application;

public static class ApplicationModule
{
    #region Methods

    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR((configuration) => configuration.RegisterServicesFromAssembly(typeof(ApplicationModule).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

        services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(RequestPostProcessor<,>));

        services.AddSingleton<ITargetsService, TargetsService>();
    }

    #endregion
}

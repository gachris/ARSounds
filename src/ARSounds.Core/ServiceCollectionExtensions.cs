using DevToolbox.Core.ApplicationFlow;
using ARSounds.Core.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IAggregateRoot, ClientRoot>();
        services.AddSingleton<IClientRoot>(s => (ClientRoot)s.GetService<IAggregateRoot>()!);
        services.AddSingleton(s => s.GetService<IClientRoot>()!.ClaimsPrincipalState);
        services.AddSingleton(s => s.GetService<IClientRoot>()!.TargetsState);

        services.AddSingleton<IApplicationEvents, ApplicationEvents>();
        services.AddSingleton<IApplicationEventsDispatcher, ApplicationEventsDispatcher>();

        return services;
    }
}

using ARSounds.ApplicationFlow;
using ARSounds.Core.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.Core;

public static class CoreModule
{
    #region Methods

    public static void AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IDomainRoot, ClientRoot>();
        services.AddSingleton<IClientRoot>(s => (ClientRoot)s.GetService<IDomainRoot>()!);
        services.AddSingleton(s => s.GetService<IClientRoot>()!.ClaimsPrincipalState);
        services.AddSingleton(s => s.GetService<IClientRoot>()!.TargetsState);

        services.AddSingleton<IApplicationEvents, ApplicationEvents>();
        services.AddSingleton<IApplicationEventsDispatcher, ApplicationEventsDispatcher>();
    }

    #endregion
}

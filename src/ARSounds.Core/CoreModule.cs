using ARSounds.ApplicationFlow;
using ARSounds.Core.App;
using Microsoft.Extensions.DependencyInjection;

namespace ARSounds.Core;

public static class CoreModule
{
    #region Methods

    public static void AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IDomainRoot, AppRoot>();
        services.AddSingleton<IAppRoot>(s => (AppRoot)s.GetService<IDomainRoot>()!);
        services.AddSingleton(s => s.GetService<IAppRoot>()!.Targets);

        services.AddSingleton<IApplicationEvents, ApplicationEvents>();
        services.AddSingleton<IApplicationEventsDispatcher, ApplicationEventsDispatcher>();
    }

    #endregion
}

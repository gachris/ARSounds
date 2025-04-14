using ARSounds.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for configuring and enhancing the IServiceCollection with ARSounds DbContext services.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds the ARSounds ApplicationDbContext configuration.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="optionsAction">An action to configure the DbContextOptionsBuilder.</param>
    /// <returns>The updated IServiceCollection instance.</returns>
    public static IServiceCollection AddARSoundsDbContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddDbContext<ApplicationDbContext>(optionsAction, ServiceLifetime.Transient);
        return services;
    }

    /// <summary>
    /// Adds a pooled DbContext factory for the ARSounds ApplicationDbContext.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="optionsAction">An action to configure the DbContextOptionsBuilder.</param>
    /// <returns>The updated IServiceCollection instance.</returns>
    public static IServiceCollection AddARSoundsPooledDbContextFactory(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
    {
        services.AddPooledDbContextFactory<ApplicationDbContext>(optionsAction);
        return services;
    }
}

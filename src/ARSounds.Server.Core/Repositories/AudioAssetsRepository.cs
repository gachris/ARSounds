using ARSounds.EntityFramework.DbContexts;
using ARSounds.EntityFramework.Entities;
using ARSounds.Server.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Repositories;

/// <summary>
/// Repository for accessing and manipulating audio asset entities.
/// </summary>
public class AudioAssetsRepository : GenericRepository<AudioAsset>, IAudioAssetsRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioAssetsRepository"/> class using the provided application context.
    /// </summary>
    public AudioAssetsRepository(ApplicationDbContext applicationContext, ILogger<AudioAssetsRepository> logger)
        : base(applicationContext, logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioAssetsRepository"/> class using a context factory.
    /// </summary>
    public AudioAssetsRepository(IDbContextFactory<ApplicationDbContext> applicationContextPool, ILogger<AudioAssetsRepository> logger)
        : base(applicationContextPool, logger)
    {
    }
}

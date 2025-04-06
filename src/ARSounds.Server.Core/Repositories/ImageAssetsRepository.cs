using ARSounds.EntityFramework.DbContexts;
using ARSounds.EntityFramework.Entities;
using ARSounds.Server.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ARSounds.Server.Core.Repositories;

/// <summary>
/// Repository for accessing and manipulating image asset entities.
/// </summary>
public class ImageAssetsRepository : GenericRepository<ImageAsset>, IImageAssetsRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ImageAssetsRepository"/> class using the provided application context.
    /// </summary>
    public ImageAssetsRepository(ApplicationDbContext applicationContext, ILogger<ImageAssetsRepository> logger)
        : base(applicationContext, logger)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ImageAssetsRepository"/> class using a context factory.
    /// </summary>
    public ImageAssetsRepository(IDbContextFactory<ApplicationDbContext> applicationContextPool, ILogger<ImageAssetsRepository> logger)
        : base(applicationContextPool, logger)
    {
    }
}

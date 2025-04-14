using ARSounds.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace ARSounds.EntityFramework.DbContexts;

/// <summary>
/// Represents the database context for the ARSounds application.
/// </summary>
public class ApplicationDbContext : DbContext
{
    #region Properties

    /// <summary>
    /// Gets or sets the audio assets.
    /// </summary>
    public virtual DbSet<AudioAsset> AudioAssets { get; set; }

    /// <summary>
    /// Gets or sets the image assets.
    /// </summary>
    public virtual DbSet<ImageAsset> ImageAssets { get; set; }

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    #region Methods Overrides

    /// <summary>
    /// Configures the model relationships and behaviors.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure one-to-one relationship between AudioAsset and ImageAsset.
        // An AudioAsset has one ImageAsset and vice versa.
        // The foreign key is defined on the ImageAsset entity.
        modelBuilder.Entity<AudioAsset>()
            .HasOne(i => i.ImageAsset)
            .WithOne(t => t.AudioAsset)
            .HasForeignKey<ImageAsset>(i => i.AudioAssetId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure the same relationship explicitly on ImageAsset as well.
        // This configuration is redundant if already configured above,
        // but sometimes included for clarity.
        modelBuilder.Entity<ImageAsset>()
            .HasOne(i => i.AudioAsset)
            .WithOne(t => t.ImageAsset)
            .HasForeignKey<ImageAsset>(i => i.AudioAssetId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }

    #endregion
}

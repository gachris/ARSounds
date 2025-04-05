using ARSounds.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace ARSounds.EntityFramework.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Target> Target { get; set; }

    public DbSet<Audio> Audio { get; set; }

    public DbSet<Image> Image { get; set; }
}

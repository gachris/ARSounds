using ARSounds.Web.Api.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace ARSounds.Web.Api.EntityFramework.DbContexts;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Target> Target { get; set; }

    public DbSet<Audio> Audio { get; set; }

    public DbSet<Image> Image { get; set; }
}

using Microsoft.EntityFrameworkCore;
using projects_api.Data.Entities;

namespace projects_api.Data;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions options)
        : base(options)
    {
    }
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>().Property(i => i.SizeInBytes).HasColumnType("decimal(18, 2)");
        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Sugar Learning", Author = "Luke Cook", SizeInBytes = 34.5m, IsCompleted = true },
            new Project { Id = 2, Name = "TimePro", Author = "Ben Neoh", SizeInBytes = 234.8m, IsCompleted = false },
            new Project { Id = 3, Name = "Microsoft Teams", Author = "Jeoffrey Fischer", SizeInBytes = 23m, IsCompleted = false }
        );
        base.OnModelCreating(modelBuilder);
    }
}
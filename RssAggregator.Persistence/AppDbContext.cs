using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Feed> Feeds { get; set; }
    public DbSet<Post> Posts { get; set; }

    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =
            configuration.GetConnectionString("Postgres");

        optionsBuilder
            .UseNpgsql(connectionString)
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    private static ILoggerFactory CreateLoggerFactory()
    {
        return LoggerFactory.Create(builder => builder.AddConsole());
    }
}
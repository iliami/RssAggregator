using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence;

public class AppDbContext(IConfiguration configuration) : DbContext, IAppDbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Feed> Feeds { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ASPNETCORE_RSSAGGREGATOR_DATABASE_CONNECTIONSTRING") ??
                               configuration.GetConnectionString("DevelopmentPostgres");
        
        optionsBuilder
            .UseNpgsql(connectionString)
            .UseLoggerFactory(CreateLoggerFactory())
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    private static ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => builder.AddConsole());

}
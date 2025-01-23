using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Tests;

public class TestDbContext() : AppDbContext(Configuration, LoggerFactory)
{
    private static readonly IConfiguration Configuration = null!;
    private static readonly ILoggerFactory LoggerFactory = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .Property(p => p.Id)
            .HasValueGenerator(typeof(GuidValueGenerator));
        modelBuilder.Entity<Feed>()
            .Property(p => p.Id)
            .HasValueGenerator(typeof(GuidValueGenerator));
        modelBuilder.Entity<Post>()
            .Property(p => p.Id)
            .HasValueGenerator(typeof(GuidValueGenerator));
    }
}
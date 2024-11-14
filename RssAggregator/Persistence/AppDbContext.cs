using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Feed> Feeds { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>()
            .HasMany(u => u.Feeds)
            .WithMany(f => f.Users)
            .UsingEntity<Subscription>();

        modelBuilder.Entity<Subscription>()
            .HasKey(s => new { s.AppUserId, s.FeedId });
        
        base.OnModelCreating(modelBuilder);
    }
}
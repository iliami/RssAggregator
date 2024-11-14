using Microsoft.EntityFrameworkCore;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions;

public interface IAppDbContext
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<Feed> Feeds { get; set; }
    public DbSet<Post> Posts { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
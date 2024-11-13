using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Persistence.Entities;

public class Feed
{
    public Guid Id { get; set; }
    [StringLength(128)]
    public required string Name { get; set; }
    [StringLength(256)]
    public required string Url { get; set; }
    public DateTime? LastFetchedAt { get; set; }

    public List<Subscription> Subscriptions { get; set; } = [];
    public List<AppUser> Users { get; set; } = [];
    
    public List<Post> Posts { get; } = [];
}
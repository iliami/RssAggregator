using System.ComponentModel.DataAnnotations;

namespace RssAggregator.Domain.Entities;

public class Feed
{
    public Guid Id { get; set; }
    [StringLength(128)]
    public required string Name { get; set; }
    [StringLength(256)]
    public required string Url { get; set; }
    [StringLength(2048)]
    public string Description { get; set; } = string.Empty;
    public DateTime? LastFetchedAt { get; set; }

    public List<Subscription> Subscriptions { get; set; } = [];
    public List<AppUser> Users { get; set; } = [];
    
    public List<Post> Posts { get; } = [];
}
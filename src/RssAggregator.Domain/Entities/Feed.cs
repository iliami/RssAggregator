namespace RssAggregator.Domain.Entities;

public class Feed
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Url { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset? LastFetchedAt { get; set; }
    public ICollection<User> Subscribers { get; set; } = [];
    public ICollection<Post> Posts { get; set; } = [];
}
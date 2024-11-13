namespace RssAggregator.Persistence.Entities;

public class Subscription
{
    public Guid AppUserId { get; set; }
    public required AppUser AppUser { get; set; }
    
    public Guid FeedId { get; set; }
    public required Feed Feed { get; set; }
    
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}
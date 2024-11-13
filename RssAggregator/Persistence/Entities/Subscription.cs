namespace RssAggregator.Persistence.Entities;

public class Subscription
{
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; }
    
    public Guid FeedId { get; set; }
    public Feed Feed { get; set; }
    
    public DateTime SubscribedAt { get; set; } = DateTime.UtcNow;
}
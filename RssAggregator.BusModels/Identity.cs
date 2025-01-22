namespace RssAggregator.BusModels
{
    public class Identity
    {
        public const string IdentityQueueName = "rssaggregator.identity";
        public Guid Id { get; set; }
    }
}

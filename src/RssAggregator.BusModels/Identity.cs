namespace RssAggregator.BusModels
{
    public class Identity
    {
        public const string ExchangeName = "identity.topic";
        public const string ExchangeType = "topic";
        public const string ExchangeRoutingKey = "identity.rssaggregator";
        public const string QueueName = "rssaggregator.identity";
        public const string ExchangeToQueueBindRoutingKey = "identity.#";
        public const string IdentityQueueName = "rssaggregator.identity";
        public Guid Id { get; set; }
    }
}

namespace Iliami.Identity.Domain.Constants;

public static class MQConstants
{
    public const string ExchangeName = "identity.topic";
    public const string ExchangeType = "topic";
    public const string ExchangeRoutingKey = "identity.rssaggregator";
    public const string QueueName = "rssaggregator.identity";
    public const string ExchangeToQueueBindRoutingKey = "identity.#";
}
namespace RssAggregator.Application.UseCases.Feeds.CreateFeed;

public interface ICreateFeedStorage
{
    Task<Guid> CreateFeed(string name, string url, CancellationToken ct = default);
}
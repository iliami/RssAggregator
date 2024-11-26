using System.Linq.Expressions;
using RssAggregator.Application.Abstractions.KeySelectors;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.KeySelectors;

public class FeedKeySelector : IKeySelector<Feed>
{
    public Expression<Func<Feed, object>> GetKeySelector(string? param)
        => param switch
        {
            nameof(Feed.Subscriptions) => feed => feed.Subscriptions.Count,
            nameof(Feed.Url) => feed => feed.Url,
            _ => feed => feed.Name
        };
}
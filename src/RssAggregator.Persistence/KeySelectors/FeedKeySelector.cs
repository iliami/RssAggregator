using System.Linq.Expressions;
using RssAggregator.Application;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.KeySelectors;

public class FeedKeySelector : IKeySelector<Feed>
{
    public Expression<Func<Feed, object>> GetKeySelector(string? fieldName)
    {
        return GetLowerString(fieldName) switch
        {
            var x when
                x == GetLowerString(nameof(Feed.Subscribers)) =>
                feed => feed.Subscribers.Count,
            var x when
                x == GetLowerString(nameof(Feed.Posts)) =>
                feed => feed.Posts.Count,
            var x when
                x == GetLowerString(nameof(Feed.Url)) =>
                feed => feed.Url,
            _ => feed => feed.Name
        };
    }

    private static string GetLowerString(string? s)
    {
        return s?.ToLowerInvariant() ?? string.Empty;
    }
}
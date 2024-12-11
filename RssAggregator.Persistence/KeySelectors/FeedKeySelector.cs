using System.Linq.Expressions;
using RssAggregator.Application.Abstractions.KeySelectors;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.KeySelectors;

public class FeedKeySelector : IKeySelector<Feed>
{
    public Expression<Func<Feed, object>> GetKeySelector(string? fieldName)
        => GetLowerString(fieldName) switch
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

    private string GetLowerString(string? s) => s?.ToLowerInvariant() ?? string.Empty;
}
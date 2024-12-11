using System.Linq.Expressions;
using RssAggregator.Application.Abstractions.KeySelectors;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.KeySelectors;

public class PostKeySelector : IKeySelector<Post>
{
    public Expression<Func<Post, object>> GetKeySelector(string? fieldName)
    {
        return GetLowerString(fieldName) switch
        {
            var x when x == GetLowerString(nameof(Post.Title)) => p => p.Title,
            _ => p => p.PublishDate
        };
    }

    private string GetLowerString(string? s)
    {
        return s?.ToLowerInvariant() ?? string.Empty;
    }
}
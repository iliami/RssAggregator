using System.Linq.Expressions;
using RssAggregator.Application.Abstractions.KeySelectors;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.KeySelectors;

public class PostKeySelector : IKeySelector<Post>
{
    public Expression<Func<Post, object>> GetKeySelector(string? param)
        => param switch
        {
            nameof(Post.Title) => p => p.Title,
            _ => p => p.PublishDate
        };
}
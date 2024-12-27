using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public interface IGetPostsStorage
{
    Task<Post[]> GetPosts(
        Specification<Post> specification, 
        CancellationToken ct = default);
}
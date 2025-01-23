using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetUserPosts;

public interface IGetUserPostsStorage
{
    Task<Post[]> GetUserPosts(Guid userId, Specification<Post> specification, CancellationToken ct = default);
}
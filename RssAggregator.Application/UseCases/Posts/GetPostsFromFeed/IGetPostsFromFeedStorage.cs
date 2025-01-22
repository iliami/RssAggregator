using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;

public interface IGetPostsFromFeedStorage
{
    Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default);
    Task<Post[]> GetPostsFromFeed(Guid feedId, Specification<Post> specification, CancellationToken ct = default);
}
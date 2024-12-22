using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

public interface IAddPostsInFeedStorage
{
    Task<bool> IsFeedExists(Guid feedId, CancellationToken ct = default);
    Task AddPosts(IReadOnlyCollection<Post> posts, CancellationToken ct = default);
}
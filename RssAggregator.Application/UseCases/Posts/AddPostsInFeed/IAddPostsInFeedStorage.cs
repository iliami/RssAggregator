using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

public interface IAddPostsInFeedStorage
{
    Task<bool> IsFeedExists(Guid feedId);
    Task AddPosts(IReadOnlyCollection<Post> posts, CancellationToken ct = default);
}
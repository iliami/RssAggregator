using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPost;

public interface IGetPostStorage
{
    Task<(bool success, Post post)> TryGetPost(
        Guid postId,
        Specification<Post> specification,
        CancellationToken ct = default);
}
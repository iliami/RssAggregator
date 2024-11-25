using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.DTO;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<PostDto>> GetPostsAsync(CancellationToken ct = default);
    Task<IEnumerable<PostDto>> GetByFeedIdAsync(Guid feedId, CancellationToken ct = default);
    Task<IEnumerable<PostDto>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default);
}
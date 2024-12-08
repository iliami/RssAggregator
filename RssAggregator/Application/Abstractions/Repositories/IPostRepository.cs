using RssAggregator.Application.DTO;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<PostDto[]> GetPostsAsync(PaginationParams? paginationParams = null, SortingParams? sortingParams = null, CancellationToken ct = default);
    Task<PostDto[]> GetByFeedIdAsync(Guid feedId, PaginationParams? paginationParams = null, SortingParams? sortingParams = null, CancellationToken ct = default);
    Task<PostDto[]> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null, SortingParams? sortingParams = null, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default);
}
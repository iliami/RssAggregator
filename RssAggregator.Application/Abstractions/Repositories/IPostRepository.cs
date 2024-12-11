using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface IPostRepository
{
    Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<PostUrlDto>> GetPostsUrlsAsync(CancellationToken ct = default);

    Task<PagedResult<PostDto>> GetPostsAsync(PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default);

    Task<PagedResult<PostDto>> GetByFeedIdAsync(Guid feedId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default);

    Task<PagedResult<PostDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default);

    Task AttachRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default);
}
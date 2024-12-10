using RssAggregator.Application.DTO;
using RssAggregator.Application.Params;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface IFeedRepository
{
    Task<Feed?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<FeedIdDto>> GetFeedsIdsAsync(CancellationToken ct = default);
    Task<PagedResult<FeedDto>> GetFeedsAsync(PaginationParams? paginationParams = null, SortingParams? sortParams = null, CancellationToken ct = default);
    Task<PagedResult<FeedDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null, SortingParams? sortParams = null, CancellationToken ct = default);
    Task<Guid> AddAsync(string name, string url, CancellationToken ct = default);
    Task UpdateAsync(Guid id, Feed feed, CancellationToken ct = default);
}
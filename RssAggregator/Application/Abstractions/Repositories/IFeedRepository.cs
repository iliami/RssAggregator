using RssAggregator.Application.DTO;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface IFeedRepository
{
    Task<Feed?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<FeedDto>> GetFeedsAsync(PaginationParams? paginationParams = null, SortingParams? sortParams = null, CancellationToken ct = default);
    Task<IEnumerable<FeedDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null, SortingParams? sortParams = null, CancellationToken ct = default);
    Task<Guid> AddAsync(string name, string url, CancellationToken ct = default);
    Task UpdateAsync(Guid id, Feed feed, CancellationToken ct = default);
}
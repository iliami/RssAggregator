using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.Abstractions.Repositories;

public interface ICategoryRepository
{
    Task<Category[]> GetByFeedIdWithTrackingAsync(Guid feedId, CancellationToken ct = default);
    Task AttachRangeAsync(IEnumerable<Category> categories, Guid feedId, CancellationToken ct = default);
}
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetUserFeeds;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetUserFeedsStorage(AppDbContext dbContext) : IGetUserFeedsStorage
{
    public Task<Feed[]> GetUserFeeds(Guid userId, Specification<Feed> specification, CancellationToken ct = default)
        => dbContext.Feeds
            .Where(f => f.Subscribers.Any(s => s.Id == userId))
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
}
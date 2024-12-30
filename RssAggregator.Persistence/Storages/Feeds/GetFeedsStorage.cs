using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages.Feeds;

public class GetFeedsStorage(AppDbContext dbContext) : IGetFeedsStorage
{
    public Task<Feed[]> GetFeeds(
        Specification<Feed> specification,
        CancellationToken ct = default)
        => dbContext.Feeds
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
}
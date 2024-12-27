using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetFeedsStorage(AppDbContext dbContext) : IGetFeedsStorage
{
    public Task<Feed[]> GetFeeds(
        Specification<Feed> specification, 
        CancellationToken ct = default)
        => dbContext.Feeds
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
}
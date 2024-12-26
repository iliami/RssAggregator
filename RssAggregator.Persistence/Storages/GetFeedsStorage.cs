using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetFeedsStorage(AppDbContext dbContext) : IGetFeedsStorage
{
    private static FeedKeySelector KeySelector { get; } = new();

    public Task<TProjection[]> GetFeeds<TProjection>(
        Specification<Feed, TProjection> specification, 
        CancellationToken ct = default)
    where TProjection : class
        => dbContext.Feeds
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
}
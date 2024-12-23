using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetFeedsStorage(AppDbContext dbContext) : IGetFeedsStorage
{
    private static FeedKeySelector KeySelector { get; } = new();

    public Task<PagedResult<FeedDto>> GetFeeds(
        PaginationParams paginationParams, 
        SortingParams sortingParams,
        CancellationToken ct = default)
        => dbContext.Feeds.AsNoTracking()
            .WithSorting(sortingParams, KeySelector)
            .Select(f => new FeedDto(f.Id, f.Name, f.Description, f.Url, f.Subscribers.Count, f.Posts.Count))
            .ToPagedResultAsync(paginationParams, ct);
}
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.DTO;
using RssAggregator.Application.Params;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.QueryExtensions;
namespace RssAggregator.Persistence.Repositories;

public class FeedRepository(IAppDbContext DbContext) : IFeedRepository
{
    public Task<PagedResult<FeedDto>> GetFeedsAsync(PaginationParams? paginationParams = null,
        SortingParams? sortParams = null, CancellationToken ct = default)
        => DbContext.Feeds.AsNoTracking()
            .WithSorting(sortParams, KeySelector)
            .Select(f => new FeedDto(f.Id, f.Name, f.Description, f.Url, f.Subscriptions.Count, f.Posts.Count))
            .ToPagedResultAsync(paginationParams, ct);

    private static FeedKeySelector KeySelector { get; } = new();

    public Task<Feed?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => DbContext.Feeds.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);

    public Task<PagedResult<FeedDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null,
        SortingParams? sortParams = null, CancellationToken ct = default)
        => DbContext.Feeds.AsNoTracking()
            .Where(f => DbContext.Subscriptions.Any(s => s.FeedId == f.Id))
            .WithSorting(sortParams, KeySelector)
            .Select(f => new FeedDto(f.Id, f.Name, f.Description, f.Url, f.Subscriptions.Count, f.Posts.Count))
            .ToPagedResultAsync(paginationParams, ct);

    public async Task<Guid> AddAsync(string name, string url, CancellationToken ct = default)
    {
        var feed = new Feed
        {
            Name = name,
            Url = url
        };

        await DbContext.Feeds.AddAsync(feed, ct);
        await DbContext.SaveChangesAsync(ct);

        return feed.Id;
    }

    public async Task UpdateAsync(Guid id, Feed feed, CancellationToken ct = default)
    {
        var storedFeed = await DbContext.Feeds.FirstOrDefaultAsync(f => f.Id == id, ct);
        if (storedFeed is not null)
        {
            storedFeed.Name = feed.Name;
            storedFeed.Url = feed.Url;
            storedFeed.Description = feed.Description;
            storedFeed.LastFetchedAt = feed.LastFetchedAt;

            await DbContext.SaveChangesAsync(ct);
        }
    }
}
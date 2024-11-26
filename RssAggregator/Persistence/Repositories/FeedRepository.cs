using Microsoft.EntityFrameworkCore;
using RssAggregator.Application;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.DTO;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Repositories;

public class FeedRepository(IAppDbContext DbContext) : IFeedRepository
{
    public async Task<Feed?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbContext.Feeds.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);

    public async Task<IEnumerable<FeedDto>> GetFeedsAsync(PaginationParams? paginationParams = null, CancellationToken ct = default)
        => await DbContext.Feeds.AsNoTracking()
            .Select(f => new FeedDto(f.Id, f.Name))
            .WithPagination(paginationParams)
            .ToListAsync(ct);

    public async Task<IEnumerable<FeedDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null, CancellationToken ct = default)
        => await DbContext.Feeds.AsNoTracking()
            .Where(f => DbContext.Subscriptions.Any(s => s.FeedId == f.Id))
            .Select(f => new FeedDto(f.Id, f.Name))
            .WithPagination(paginationParams)
            .ToListAsync(ct);

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

            DbContext.Feeds.Entry(storedFeed).State = EntityState.Modified;
            await DbContext.SaveChangesAsync(ct);
        }
    }
}
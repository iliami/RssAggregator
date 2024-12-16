using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Repositories;

public class CategoryRepository(AppDbContext dbContext, IMemoryCache memoryCache) : ICategoryRepository
{
    public async Task<Category[]> GetByFeedIdWithTrackingAsync(Guid feedId, CancellationToken ct = default)
    {
        var categories = await memoryCache.GetOrCreateAsync<Category[]>($"categories_{feedId}", async entry =>
        {
            var categories = await dbContext.Categories.Where(c => c.Feed.Id == feedId).ToArrayAsync(ct);
            entry.Value = categories;
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
            return categories;
        }) ?? [];

        return categories;
    }

    public async Task AttachRangeAsync(IEnumerable<Category> categories, Guid feedId, CancellationToken ct = default)
    {
        dbContext.Categories.AttachRange(categories);

        var added = await dbContext.SaveChangesAsync(ct);
        
        if (added > 0)
        {
            memoryCache.Remove($"categories_{feedId}");
        }
    }
}
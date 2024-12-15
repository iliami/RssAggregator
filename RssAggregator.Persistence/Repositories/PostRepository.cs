using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Repositories;

public class PostRepository(AppDbContext DbContext, IMemoryCache memoryCache) : IPostRepository
{
    private static PostKeySelector PostKeySelector { get; } = new();

    public Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return DbContext.Posts
            .AsNoTracking()
            .Include(p => p.Feed)
            .Include(p => p.Categories)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public Task<List<PostUrlDto>> GetPostsUrlsAsync(CancellationToken ct = default)
    {
        return DbContext.Posts.AsNoTracking()
            .Select(p => new PostUrlDto(p.Id, p.Url, p.Feed.Id))
            .ToListAsync(ct);
    }

    public Task<PagedResult<PostDto>> GetPostsAsync(PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
    {
        return DbContext.Posts.AsNoTracking()
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, string.Join(", ", p.Categories), p.PublishDate, p.Url, p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);
    }

    public Task<PagedResult<PostDto>> GetByFeedIdAsync(Guid feedId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
    {
        return DbContext.Posts.AsNoTracking()
            .Where(p => p.Feed.Id == feedId)
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, string.Join(", ", p.Categories), p.PublishDate, p.Url, p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);
    }

    public Task<PagedResult<PostDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
    {
        return DbContext.Posts.AsNoTracking()
            .Where(p => p.Feed.Subscribers.Any(u => u.Id == userId))
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, string.Join(", ", p.Categories), p.PublishDate, p.Url, p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);
    }

    public async Task AttachRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default)
    {
        var storedCategories = await memoryCache.GetOrCreateAsync<Category[]>("categories", async entry =>
        {
            var categories = await DbContext.Categories.AsNoTracking().ToArrayAsync(ct);
            entry.Value = categories;
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
            return categories;
        });

        var postsToAttach = posts as Post[] ?? posts.ToArray();
        
        var categories = postsToAttach
            .Select(p => p.Categories
                .Select(c =>
                {
                    if (storedCategories is null) return c;

                    var storedCategory = storedCategories.FirstOrDefault(x => x.Name == c.Name);

                    if (storedCategory is not null)
                    {
                        c.Id = storedCategory.Id;
                    };

                    return c;
                }))
            .Aggregate(
                (acc, seq) => 
                    acc.Concat(seq).ToArray())
            .DistinctBy(x => x.Name)
            .ToArray();
        
        DbContext.Categories.AttachRange(categories);
        
        var added = await DbContext.SaveChangesAsync(ct);

        if (added > 0)
        {
            memoryCache.Remove("categories");
        }

        DbContext.Posts.AttachRange(postsToAttach);
        
        await DbContext.SaveChangesAsync(ct);
    }
}
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Repositories;

public class PostRepository(AppDbContext DbContext) : IPostRepository
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

    public Task<PagedResult<PostDto>> GetPostsAsync(
        PaginationParams paginationParams,
        SortingParams sortingParams,
        PostFilterParams filterParams, 
        CancellationToken ct = default)
    {
        return DbContext.Posts.AsNoTracking()
            .WithFiltration(filterParams)
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(
                p.Id, 
                p.Title, 
                p.Categories.Select(c => c.Name).ToArray(), 
                p.PublishDate, 
                p.Url, 
                p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);
    }

    public Task<PagedResult<PostDto>> GetByFeedIdAsync(Guid feedId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, PostFilterParams? filterParams = null, CancellationToken ct = default)
    {
        return DbContext.Posts.AsNoTracking()
            .Where(p => p.Feed.Id == feedId)
            .WithFiltration(filterParams)
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(
                p.Id, 
                p.Title, 
                p.Categories.Select(c => c.Name).ToArray(), 
                p.PublishDate, 
                p.Url, 
                p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);
    }

    public Task<PagedResult<PostDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, PostFilterParams? filterParams = null, CancellationToken ct = default)
    {
        return DbContext.Posts.AsNoTracking()
            .Where(p => p.Feed.Subscribers.Any(u => u.Id == userId))
            .WithFiltration(filterParams)
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(
                p.Id, 
                p.Title, 
                p.Categories.Select(c => c.Name).ToArray(), 
                p.PublishDate, 
                p.Url, 
                p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);
    }

    public async Task AttachRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default)
    {
        DbContext.Posts.AttachRange(posts);
        
        await DbContext.SaveChangesAsync(ct);
    }
}
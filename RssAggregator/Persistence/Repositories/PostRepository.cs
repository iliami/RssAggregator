using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Repositories;

public class PostRepository(IAppDbContext DbContext) : IPostRepository
{
    private static PostKeySelector PostKeySelector { get; } = new();

    public Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => DbContext.Posts
            .AsNoTracking()
            .Include(p => p.Feed)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<List<PostUrlDto>> GetPostsUrlsAsync(CancellationToken ct = default)
        => DbContext.Posts.AsNoTracking()
            .Select(p => new PostUrlDto(p.Id, p.Url, p.Feed.Id))
            .ToListAsync(ct);
    
    public Task<PagedResult<PostDto>> GetPostsAsync(PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
        => DbContext.Posts.AsNoTracking()
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, p.Category, p.PublishDate, p.Url, p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);

    public Task<PagedResult<PostDto>> GetByFeedIdAsync(Guid feedId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
        => DbContext.Posts.AsNoTracking()
            .Where(p => p.Feed.Id == feedId)
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, p.Category, p.PublishDate, p.Url, p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);

    public Task<PagedResult<PostDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
        => DbContext.Posts.AsNoTracking()
            .Where(p => p.Feed.Subscribers.Any(u => u.Id == userId))
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, p.Category, p.PublishDate, p.Url, p.Feed.Id))
            .ToPagedResultAsync(paginationParams, ct);

    public async Task AttachRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default)
    {
        DbContext.Posts.AttachRange(posts);
        await DbContext.SaveChangesAsync(ct);
    }
}
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

public class PostRepository(IAppDbContext DbContext) : IPostRepository
{
    private static PostKeySelector PostKeySelector { get; } = new();

    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbContext.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<PagedResult<PostDto>> GetPostsAsync(PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
        => DbContext.Posts.AsNoTracking()
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, p.Category, p.PublishDate, p.Url, p.FeedId))
            .ToPagedResultAsync(paginationParams, ct);

    public Task<PagedResult<PostDto>> GetByFeedIdAsync(Guid feedId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
        => DbContext.Posts.AsNoTracking()
            .Where(p => p.FeedId == feedId)
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, p.Category, p.PublishDate, p.Url, p.FeedId))
            .ToPagedResultAsync(paginationParams, ct);

    public Task<PagedResult<PostDto>> GetByUserIdAsync(Guid userId, PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null, CancellationToken ct = default)
        => DbContext.Posts.AsNoTracking()
            .Where(p => DbContext.Subscriptions
                .Any(s => s.AppUserId == userId && s.FeedId == p.FeedId))
            .WithSorting(sortingParams, PostKeySelector)
            .Select(p => new PostDto(p.Id, p.Title, p.Category, p.PublishDate, p.Url, p.FeedId))
            .ToPagedResultAsync(paginationParams, ct);

    public async Task AddRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default)
    {
        await DbContext.Posts.AddRangeAsync(posts, ct);
        await DbContext.SaveChangesAsync(ct);
    }
}
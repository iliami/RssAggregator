using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Domain.Entities;
using RssAggregator.Presentation.DTO;

namespace RssAggregator.Persistence.Repositories;

public class PostRepository(IAppDbContext DbContext) : IPostRepository
{
    public async Task<Post?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await DbContext.Posts.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<IEnumerable<PostDto>> GetPostsAsync(CancellationToken ct = default)
        => await DbContext.Posts.AsNoTracking()
            .Select(p => new PostDto(p.Id, p.Title, p.PublishDate, p.Url, p.FeedId))
            .ToListAsync(ct);
    
    public async Task<IEnumerable<PostDto>> GetByFeedIdAsync(Guid feedId, CancellationToken ct = default)
        => await DbContext.Posts.AsNoTracking()
            .Where(p => p.FeedId == feedId)
            .Select(p => new PostDto(p.Id, p.Title, p.PublishDate, p.Url, p.FeedId))
            .ToListAsync(ct);

    public async Task<IEnumerable<PostDto>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        => await DbContext.Posts
            .AsNoTracking()
            .Where(p => DbContext.Subscriptions
                .Any(s => s.AppUserId == userId && s.FeedId == p.FeedId))
            .Select(p => new PostDto(
                p.Id,
                p.Title,
                p.PublishDate,
                p.Url,
                p.FeedId))
            .ToListAsync(ct);
    
    public async Task AddRangeAsync(IEnumerable<Post> posts, CancellationToken ct = default)
        => await DbContext.Posts.AddRangeAsync(posts, ct);
}
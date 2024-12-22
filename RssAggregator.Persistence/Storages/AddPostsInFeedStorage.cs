using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Posts.AddPostsInFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages;

public class AddPostsInFeedStorage(AppDbContext dbContext) : IAddPostsInFeedStorage
{
    public Task<bool> IsFeedExists(Guid feedId, CancellationToken ct = default)
        => dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);

    public async Task AddPosts(IReadOnlyCollection<Post> posts, CancellationToken ct = default)
    {
        dbContext.AttachRange(posts);
        await dbContext.SaveChangesAsync(ct);
    }
}
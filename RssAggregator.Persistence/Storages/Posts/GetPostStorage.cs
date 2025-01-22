using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Persistence.Storages.Posts;

public class GetPostStorage(AppDbContext dbContext) : IGetPostStorage
{
    public async Task<(bool success, Post post)> TryGetPost(Guid id, CancellationToken ct = default)
    {
        var post = await dbContext.Posts
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (post is null)
        {
            return (false, null!);
        }

        await dbContext.Posts
            .Entry(post)
            .Reference(p => p.Feed)
            .LoadAsync(ct);

        await dbContext.Posts
            .Entry(post)
            .Collection(p => p.Categories)
            .LoadAsync(ct);

        return (true, post);
    }
}
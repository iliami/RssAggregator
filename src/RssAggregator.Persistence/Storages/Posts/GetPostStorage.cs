using Microsoft.EntityFrameworkCore;
using RssAggregator.Application;
using RssAggregator.Application.UseCases.Posts.GetPost;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages.Posts;

public class GetPostStorage(AppDbContext dbContext) : IGetPostStorage
{
    public async Task<(bool success, Post post)> TryGetPost(
        Guid postId, 
        Specification<Post> specification, 
        CancellationToken ct = default)
    {
        var isPostExists = dbContext.Posts.Any(p => p.Id == postId);
        if (!isPostExists)
        {
            return (false, null!);
        }
        
        var post = await dbContext.Posts
            .Where(x => x.Id == postId)
            .EvaluateSpecification(specification)
            .FirstAsync(ct);

        return (true, post);
    }
}
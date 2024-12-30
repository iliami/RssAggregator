using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Specifications;
using RssAggregator.Application.UseCases.Posts.GetUserPosts;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages.Posts;

public class GetUserPostsStorage(AppDbContext dbContext) : IGetUserPostsStorage
{
    public Task<Post[]> GetUserPosts(Guid userId, Specification<Post> specification, CancellationToken ct = default)
        => dbContext.Posts
            .Where(p => p.Feed.Subscribers.Any(s => s.Id == userId))
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
}
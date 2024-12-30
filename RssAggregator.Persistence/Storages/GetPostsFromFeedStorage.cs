using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetPostsFromFeedStorage(AppDbContext dbContext) : IGetPostsFromFeedStorage
{
    public Task<bool> IsFeedExist(Guid feedId, CancellationToken ct = default)
        => dbContext.Feeds.AnyAsync(f => f.Id == feedId, ct);

    public Task<Post[]> GetPostsFromFeed(Guid feedId, Specification<Post> specification, CancellationToken ct = default)
        => dbContext.Posts
            .Where(p => p.Feed.Id == feedId)
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
}
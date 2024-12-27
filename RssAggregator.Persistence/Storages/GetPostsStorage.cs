using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetPostsStorage(AppDbContext dbContext) : IGetPostsStorage
{ 
    public Task<Post[]> GetPosts(
        Specification<Post> specification, 
        CancellationToken ct = default)
    {
        return dbContext.Posts
            .EvaluateSpecification(specification)
            .ToArrayAsync(ct);
    }
}
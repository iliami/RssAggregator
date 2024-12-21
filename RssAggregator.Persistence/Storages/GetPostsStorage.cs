using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Persistence.KeySelectors;
using RssAggregator.Persistence.QueryExtensions;

namespace RssAggregator.Persistence.Storages;

public class GetPostsStorage(AppDbContext dbContext) : IGetPostsStorage
{
    private static PostKeySelector PostKeySelector { get; } = new();

    public Task<PagedResult<PostDto>> GetPosts(
        PaginationParams paginationParams, 
        SortingParams sortingParams,
        PostFilterParams filterParams, 
        CancellationToken ct = default)
    {
        return dbContext.Posts.AsNoTracking()
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
}
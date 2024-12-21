using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public interface IGetPostsStorage
{
    Task<PagedResult<PostDto>> GetPosts(
        PaginationParams paginationParams,
        SortingParams sortingParams,
        PostFilterParams filterParams, 
        CancellationToken ct = default);
}
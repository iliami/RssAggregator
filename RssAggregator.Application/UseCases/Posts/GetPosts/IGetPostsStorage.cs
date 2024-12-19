using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public interface IGetPostsStorage
{
    Task<PagedResult<PostDto>> GetPosts(
        PaginationParams? paginationParams = null,
        SortingParams? sortingParams = null,
        PostFilterParams? filterParams = null, 
        CancellationToken ct = default);
}
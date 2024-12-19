using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public record GetPostsRequest(
    PaginationParams? PaginationParams = null,
    SortingParams? SortingParams = null,
    PostFilterParams? FilterParams = null);
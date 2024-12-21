using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public record GetPostsRequest(
    PaginationParams PaginationParams,
    SortingParams SortingParams,
    PostFilterParams FilterParams);
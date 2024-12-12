// using FastEndpoints;
// using RssAggregator.Application.Abstractions.Repositories;
// using RssAggregator.Application.Models.DTO;
// using RssAggregator.Application.Models.Params;
//
// namespace RssAggregator.Presentation.Endpoints.Api;
//
// public record GetFeedPostsRequest(
//     Guid FeedId,
//     int Page = 1,
//     int PageSize = 50,
//     string? SortBy = null,
//     SortDirection SortDirection = SortDirection.None);
//
// public record GetFeedPostsResponse(PagedResult<PostDto> Posts);
//
// public class GetFeedPostsEndpoint(IPostRepository PostRepository) : Endpoint<GetFeedPostsRequest, GetFeedPostsResponse>
// {
//     public override void Configure()
//     {
//         Get("api/posts");
//     }
//
//     public override async Task<GetFeedPostsResponse> ExecuteAsync(GetFeedPostsRequest req, CancellationToken ct)
//     {
//         var feedId = req.FeedId;
//         var paginationParams = new PaginationParams
//         {
//             Page = req.Page,
//             PageSize = req.PageSize
//         };
//
//         var sortingParams = new SortingParams
//         {
//             SortBy = req.SortBy,
//             SortDirection = req.SortDirection
//         };
//
//         var posts = await PostRepository.GetByFeedIdAsync(feedId, paginationParams, sortingParams, ct);
//
//         return new GetFeedPostsResponse(posts);
//     }
// }
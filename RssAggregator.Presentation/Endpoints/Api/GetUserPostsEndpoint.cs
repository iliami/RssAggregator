// using FastEndpoints;
// using RssAggregator.Application.Abstractions.Repositories;
// using RssAggregator.Application.Models.DTO;
// using RssAggregator.Application.Models.Params;
// using RssAggregator.Presentation.Extensions;
//
// namespace RssAggregator.Presentation.Endpoints.Api;
//
// public record GetUserPostsRequest(
//     int Page = 1,
//     int PageSize = 50,
//     string? SortBy = null,
//     SortDirection SortDirection = SortDirection.None);
//
// public record GetUserPostsResponse(PagedResult<PostDto> Posts);
//
// public class GetUserPostsEndpoint(IPostRepository PostRepository) : Endpoint<GetUserPostsRequest, GetUserPostsResponse>
// {
//     public override void Configure()
//     {
//         Get("api/posts/me");
//     }
//
//     public override async Task<GetUserPostsResponse> ExecuteAsync(GetUserPostsRequest req, CancellationToken ct)
//     {
//         var (userId, _) = User.ToIdEmailTuple();
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
//         var posts = await PostRepository.GetByUserIdAsync(userId, paginationParams, sortingParams, ct);
//
//         return new GetUserPostsResponse(posts);
//     }
// }
// using FastEndpoints;
// using RssAggregator.Application.Abstractions.Repositories;
// using RssAggregator.Application.Models.DTO;
// using RssAggregator.Application.Models.Params;
//
// namespace RssAggregator.Presentation.Endpoints.Api;
//
// public record GetAllFeedsRequest(
//     int Page = 1,
//     int PageSize = 10,
//     string? SortBy = null,
//     SortDirection SortDirection = SortDirection.None);
//
// public record GetAllFeedsResponse(PagedResult<FeedDto> Feeds);
//
// public class GetAllFeedsEndpoint(IFeedRepository FeedRepository)
//     : Endpoint<GetAllFeedsRequest, GetAllFeedsResponse>
// {
//     public override void Configure()
//     {
//         Get("api/feeds");
//     }
//
//     public override async Task<GetAllFeedsResponse> ExecuteAsync(GetAllFeedsRequest req, CancellationToken ct)
//     {
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
//         var feeds = await FeedRepository.GetFeedsAsync(paginationParams, sortingParams, ct);
//
//         return new GetAllFeedsResponse(feeds);
//     }
// }
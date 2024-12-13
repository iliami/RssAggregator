using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public record GetFeedsResponse(PagedResult<FeedDto> Feeds);

public class GetFeeds : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromServices] IFeedRepository feedRepository,
            CancellationToken ct) =>
        {
            var feeds = await feedRepository.GetFeedsAsync(
                paginationParams, 
                sortingParams, ct);
            
            var response = new GetFeedsResponse(feeds);
            
            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Feeds);
    }
}
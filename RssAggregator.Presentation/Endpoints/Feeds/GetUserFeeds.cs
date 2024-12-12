using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public record GetUserFeedsResponse(PagedResult<FeedDto> Feeds);

public class GetUserFeeds : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds/me", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromServices] IFeedRepository feedRepository,
            [FromServices] HttpContext context,
            [FromServices] CancellationToken ct) =>
        {
            var (userId, _) = context.User.ToIdEmailTuple();
            
            var feeds = await feedRepository.GetByUserIdAsync(
                userId, 
                paginationParams, 
                sortingParams, 
                ct);
            
            var response = new GetUserFeedsResponse(feeds);
            
            return Results.Ok(response);
        }).WithTags(Tags.Feeds);
    }
}
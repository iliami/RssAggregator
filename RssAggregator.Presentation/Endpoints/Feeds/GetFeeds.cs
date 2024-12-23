using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public class GetFeeds : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromServices] IGetFeedsUseCase useCase,
            CancellationToken ct) =>
        {
            var request = new GetFeedsRequest(paginationParams, sortingParams);
            var response = await useCase.Handle(request, ct);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Feeds);
    }
}
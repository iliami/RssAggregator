using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.KeySelectors;
using RssAggregator.Application.Params;
using RssAggregator.Application.Specifications;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public class GetFeeds : IEndpoint
{
    private record GetFeedsModel(Guid Id, string Name, string Url, int Posts, int Subscribers);

    private class GetFeedsSpecification : Specification<Feed>
    {
        public GetFeedsSpecification(
            IKeySelector<Feed> keySelector,
            PaginationParams paginationParams,
            SortingParams sortingParams)
        {
            IsNoTracking = true;

            Skip = (paginationParams.Page - 1) * paginationParams.PageSize;
            Take = paginationParams.PageSize;

            AddInclude(feed => feed.Posts);
            AddInclude(feed => feed.Subscribers);

            if (sortingParams.SortDirection == SortDirection.None)
            {
                return;
            }

            var selector = keySelector.GetKeySelector(sortingParams.SortBy);

            if (sortingParams.SortDirection == SortDirection.Asc)
            {
                SetAscendingOrderBy(selector);
            }
            else
            {
                SetDescendingOrderBy(selector);
            }
        }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("feeds", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromServices] IGetFeedsUseCase useCase,
            [FromServices] IKeySelector<Feed> selector,
            CancellationToken ct) =>
        {
            var request = new GetFeedsRequest(new GetFeedsSpecification(selector, paginationParams, sortingParams));

            var response = await useCase.Handle(request, ct);

            var feeds = response.Feeds.Select(
                feed => new GetFeedsModel(
                    feed.Id,
                    feed.Name,
                    feed.Url,
                    feed.Posts.Count,
                    feed.Subscribers.Count));

            return Results.Ok(feeds);
        }).AllowAnonymous().WithTags(EndpointsTags.Feeds);
    }
}
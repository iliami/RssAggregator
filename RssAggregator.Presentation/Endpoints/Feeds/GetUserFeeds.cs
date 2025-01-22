using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application;
using RssAggregator.Application.Params;
using RssAggregator.Application.UseCases.Feeds.GetUserFeeds;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public class GetUserFeeds : IEndpoint
{
    private record GetUserFeedsModel(Guid Id, string Name, string Url, int Posts, int Subscribers);

    private class GetUserFeedsSpecification : Specification<Feed>
    {
        public GetUserFeedsSpecification(
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
        app.MapGet("feeds/me", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromServices] IGetUserFeedsUseCase useCase,
            [FromServices] IKeySelector<Feed> selector,
            CancellationToken ct) =>
        {
            var specification = new GetUserFeedsSpecification(
                selector,
                paginationParams,
                sortingParams);

            var request = new GetUserFeedsRequest(specification);

            var response = await useCase.Handle(request, ct);

            var feeds = response.Feeds.Select(
                feed => new GetUserFeedsModel(
                    feed.Id,
                    feed.Name,
                    feed.Url,
                    feed.Posts.Count,
                    feed.Subscribers.Count
                ));

            return Results.Ok(feeds);
        }).RequireAuthorization().WithTags(EndpointsTags.Feeds);
    }
}
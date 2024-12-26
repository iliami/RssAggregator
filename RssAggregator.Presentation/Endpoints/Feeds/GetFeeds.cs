using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Feeds.GetFeeds;
using RssAggregator.Domain.Entities;
using RssAggregator.Persistence.KeySelectors;

namespace RssAggregator.Presentation.Endpoints.Feeds;

public class GetFeeds : IEndpoint
{
    private record GetFeedsResponseModel(Guid Id, string Name, string Url, int Posts, int Subscribers);

    private class GetFeedsSpecification : Specification<Feed, GetFeedsResponseModel>
    {
        private static readonly FeedKeySelector Selector = new();
        public GetFeedsSpecification(
            PaginationParams paginationParams,
            SortingParams sortingParams)
            : base(feed => new GetFeedsResponseModel(
                feed.Id,
                feed.Name,
                feed.Url,
                feed.Posts.Count,
                feed.Subscribers.Count))
        {
            IsNoTracking = true;
            
            Skip = (paginationParams.Page - 1) * paginationParams.PageSize;
            Take = paginationParams.PageSize;

            if (sortingParams.SortDirection == SortDirection.None)
            {
                return;
            }
            
            var selector = Selector.GetKeySelector(sortingParams.SortBy);

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
            [FromServices] IGetFeedsUseCase<GetFeedsResponseModel> useCase,
            CancellationToken ct) =>
        {
            var req = new GetFeedsRequest<GetFeedsResponseModel>(new GetFeedsSpecification(paginationParams, sortingParams));

            var response = await useCase.Handle(req, ct);

            return Results.Ok(response);
        }).AllowAnonymous().WithTags(EndpointsTags.Feeds);
    }
}
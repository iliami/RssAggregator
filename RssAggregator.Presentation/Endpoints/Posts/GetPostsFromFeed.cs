using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.Abstractions.KeySelectors;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Presentation.Endpoints.Posts;

public record GetPostsResponse(PagedResult<PostDto> Posts);

public class GetPostsFromFeed : IEndpoint
{
    private record GetPostsFromFeedModel(Guid Id, string Title, IEnumerable<string> Categories, DateTime PublishDate, string Url, Guid FeedId);

    private class GetPostsFromFeedSpecification : Specification<Post>
    {
        public GetPostsFromFeedSpecification(
            IKeySelector<Post> keySelector,
            PaginationParams paginationParams,
            SortingParams sortingParams,
            PostFilterParams filterParams)
        {
            IsNoTracking = true;
            
            Skip = (paginationParams.Page - 1) * paginationParams.PageSize;
            Take = paginationParams.PageSize;
            
            AddInclude(post => post.Categories);
            AddInclude(post => post.Feed);
            
            Criteria = post => filterParams.Categories
                .Select(c => c.ToLowerInvariant())
                .All(category => post.Categories
                    .Any(c => c.NormalizedName == category));

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
        app.MapGet("feeds/{id:guid}/posts", async (
            [FromRoute]    Guid id,
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromQuery] string[]? categories,
            [FromServices] IGetPostsFromFeedUseCase useCase,
            [FromServices] IKeySelector<Post> selector,
            CancellationToken ct) =>
        {
            var filterParams = new PostFilterParams(categories ?? []);
            var specification = new GetPostsFromFeedSpecification(
                selector,
                paginationParams,
                sortingParams,
                filterParams);
            var request = new GetPostsFromFeedRequest(id, specification);
            
            var response = await useCase.Handle(request, ct);
            
            var posts = response.Posts.Select(post => new GetPostsFromFeedModel(
                post.Id,
                post.Title,
                post.Categories.Select(c => c.Name),
                post.PublishDate,
                post.Url,
                post.Feed.Id));
            
            return Results.Ok(posts);
        }).AllowAnonymous().WithTags(EndpointsTags.Posts);
    }
}
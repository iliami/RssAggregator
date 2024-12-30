using Microsoft.AspNetCore.Mvc;
using RssAggregator.Application.KeySelectors;
using RssAggregator.Application.Params;
using RssAggregator.Application.Specifications;
using RssAggregator.Application.UseCases.Posts.GetPosts;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Presentation.Endpoints.Posts;

public class GetPosts : IEndpoint
{
    private record GetPostsModel(
        Guid Id,
        string Title,
        IEnumerable<string> Categories,
        DateTime PublishDate,
        string Url,
        Guid FeedId);

    private class GetPostsSpecification : Specification<Post>
    {
        public GetPostsSpecification(
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
        app.MapGet("posts", async (
            [AsParameters] PaginationParams paginationParams,
            [AsParameters] SortingParams sortingParams,
            [FromQuery] string[]? categories,
            [FromServices] IGetPostsUseCase useCase,
            [FromServices] IKeySelector<Post> selector,
            CancellationToken ct) =>
        {
            var filterParams = new PostFilterParams(categories ?? []);
            var specification = new GetPostsSpecification(
                selector,
                paginationParams,
                sortingParams,
                filterParams);
            var request = new GetPostsRequest(specification);

            var response = await useCase.Handle(request, ct);

            var posts = response.Posts.Select(post => new GetPostsModel(
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
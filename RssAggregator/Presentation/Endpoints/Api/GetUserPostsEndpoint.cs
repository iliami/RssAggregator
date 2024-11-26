using FastEndpoints;
using RssAggregator.Application;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserPostsEndpoint(IPostRepository PostRepository) : Endpoint<GetUserPostsRequest, GetUserPostsResponse>
{
    public override void Configure()
    {
        Get("api/posts/me");
    }

    public override async Task<GetUserPostsResponse> ExecuteAsync(GetUserPostsRequest req, CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();
        var paginationParams = new PaginationParams
        {
            Page = req.Page,
            PageSize = req.PageSize,
        };

        var sortingParams = new SortingParams
        {
            SortBy = req.SortBy,
            SortDirection = req.SortDirection,
        };

        var posts = await PostRepository.GetByUserIdAsync(userId, paginationParams, sortingParams, ct);

        return new GetUserPostsResponse(posts);
    }
}
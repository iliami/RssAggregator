using FastEndpoints;
using RssAggregator.Application;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedPostsEndpoint(IPostRepository PostRepository) : Endpoint<GetFeedPostsRequest, GetFeedPostsResponse>
{
    public override void Configure()
    {
        Get("api/posts");
    }

    public override async Task<GetFeedPostsResponse> ExecuteAsync(GetFeedPostsRequest req, CancellationToken ct)
    {
        var feedId = req.FeedId;
        var paginationParams = new PaginationParams
        {
            Page = req.Page,
            PageSize = req.PageSize,
        };
        
        var posts = await PostRepository.GetByFeedIdAsync(feedId, paginationParams, ct);

        return new GetFeedPostsResponse(posts);
    }
}
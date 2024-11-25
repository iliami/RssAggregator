using FastEndpoints;
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
        var take = req.PageSize;
        var skip = (req.Page - 1) * req.PageSize;

        var allPosts = await PostRepository.GetByFeedIdAsync(feedId, ct);

        var posts = allPosts
            .Skip(skip)
            .Take(take);

        return new GetFeedPostsResponse(posts);
    }
}
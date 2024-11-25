using FastEndpoints;
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

        var take = req.PageSize;
        var skip = (req.Page - 1) * req.PageSize;
        
        var allPosts = await PostRepository.GetByUserIdAsync(userId, ct);

        var posts = allPosts
            .Skip(skip)
            .Take(take);
        
        return new GetUserPostsResponse(posts);
    }
}
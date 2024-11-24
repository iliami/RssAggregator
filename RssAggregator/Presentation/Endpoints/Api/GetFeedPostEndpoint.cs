using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedPostEndpoint(IAppDbContext DbContext) : EndpointWithoutRequest<GetFeedPostResponse>
{
    public override void Configure()
    {
        Get("api/posts/{id:guid}");
    }

    public override async Task<GetFeedPostResponse> ExecuteAsync(CancellationToken ct)
    {
        var postId = Route<Guid>("id");
        
        var post = await DbContext.Posts
            .FirstOrDefaultAsync(p => p.Id == postId, ct);

        if (post is null)
        {
            ThrowError("Post not found", StatusCodes.Status404NotFound);
        }
        
        var res = new GetFeedPostResponse(
            post.Id,
            post.Title,
            post.Description,
            post.PublishDate,
            post.Url,
            post.FeedId);

        return res;
    }
}
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedPostEndpoint(IAppDbContext DbContext) : EndpointWithoutRequest<GetFeedPostResponse>
{
    public override void Configure()
    {
        Get("api/feeds/{feedId:guid}/posts/{postId:guid}");
    }

    public override async Task<GetFeedPostResponse> ExecuteAsync(CancellationToken ct)
    {
        var feedId = Route<Guid>("feedId");
        var postId = Route<Guid>("postId");
        
        var post = await DbContext.Posts
            .FirstOrDefaultAsync(p => 
                p.Id == postId && p.FeedId == feedId, ct);

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
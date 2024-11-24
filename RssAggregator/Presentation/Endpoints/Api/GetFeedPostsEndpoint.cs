using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedPostsEndpoint(IAppDbContext DbContext) : Endpoint<GetFeedPostsRequest, GetFeedPostsResponse>
{
    public override void Configure()
    {
        Get("api/posts");
    }

    public override async Task<GetFeedPostsResponse> ExecuteAsync(GetFeedPostsRequest req, CancellationToken ct)
    {
        var feedId = req.FeedId;
        
        var posts = await DbContext.Posts
            .AsNoTracking()
            .Where(p => p.FeedId == feedId)
            .Select(p => new PostDto(
                p.Id,
                p.Title,
                p.PublishDate,
                p.Url,
                p.FeedId))
            .ToListAsync(ct);

        var res = new GetFeedPostsResponse(posts);

        return res;
    }
}
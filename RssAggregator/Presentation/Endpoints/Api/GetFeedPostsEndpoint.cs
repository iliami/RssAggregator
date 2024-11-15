using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetFeedPostsEndpoint(IAppDbContext DbContext) : EndpointWithoutRequest<GetFeedPostsResponse>
{
    public override void Configure()
    {
        Get("api/feeds/{id:guid}/posts");
    }

    public override async Task<GetFeedPostsResponse> ExecuteAsync(CancellationToken ct)
    {
        var feedId = Route<Guid>("id");
        
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
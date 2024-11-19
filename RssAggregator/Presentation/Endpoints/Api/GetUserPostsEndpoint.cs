using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.DTO;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserPostsEndpoint(IAppDbContext DbContext) : EndpointWithoutRequest<GetUserPostsResponse>
{
    public override void Configure()
    {
        Get("api/feeds/me/posts");
    }

    public override async Task<GetUserPostsResponse> ExecuteAsync(CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();

        var posts = await DbContext.Posts
            .AsNoTracking()
            .Where(p => DbContext.Subscriptions
                .Any(s =>
                    s.AppUserId == userId && s.FeedId == p.FeedId))
            .Select(p => new PostDto(
                p.Id,
                p.Title,
                p.PublishDate,
                p.Url,
                p.FeedId))
            .ToListAsync(ct);

        var res = new GetUserPostsResponse(posts);

        return res;
    }
}
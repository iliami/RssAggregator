using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.DTO.PostDto;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserPosts(IAppDbContext DbContext) : EndpointWithoutRequest<GetUserPostsResponse>
{
    public override void Configure()
    {
        Get("api/feeds/me/posts");
    }

    public override async Task<GetUserPostsResponse> ExecuteAsync(CancellationToken ct)
    {
        var (userId, _) = User.ToIdNameTuple();

        var posts = await DbContext.Posts
            .AsNoTracking()
            .Where(p => DbContext.Subscriptions
                .Any(s =>
                    s.AppUserId == userId && s.FeedId == p.FeedId))
            .Select(p => new PostShortDto(
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
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.DTO;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserPostsEndpoint(IAppDbContext DbContext, ISubscriptionRepository SubscriptionRepository) : Endpoint<GetUserPostsRequest, GetUserPostsResponse>
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

        var subscriptions = await SubscriptionRepository.GetByUserIdAsync(userId, ct);
        
        var posts = await DbContext.Posts
            .AsNoTracking()
            .Where(p => subscriptions
                .Any(s => s.FeedId == p.FeedId))
            .Skip(skip)
            .Take(take)
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
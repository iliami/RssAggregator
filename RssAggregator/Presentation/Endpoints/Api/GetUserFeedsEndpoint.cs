using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.DTO;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserFeedsEndpoint(IAppDbContext DbContext) : EndpointWithoutRequest<GetUserFeedsResponse>
{
    public override void Configure()
    {
        Get("api/feeds/me");
    }

    public override async Task<GetUserFeedsResponse> ExecuteAsync(CancellationToken ct)
    {
        var (userId, _) = User.ToIdNameTuple();

        var feeds = await DbContext.Feeds.AsNoTracking()
            .Where(f => f.Subscriptions.Any(s => s.AppUserId == userId))
            .Select(f => new FeedDto(f.Id, f.Name))
            .ToListAsync(ct);
        
        return new GetUserFeedsResponse(feeds);
    }
}
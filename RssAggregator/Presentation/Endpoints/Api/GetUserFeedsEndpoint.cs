using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.DTO;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserFeedsEndpoint(IAppDbContext DbContext, ISubscriptionRepository SubscriptionRepository) : EndpointWithoutRequest<GetUserFeedsResponse>
{
    public override void Configure()
    {
        Get("api/feeds/me");
    }

    public override async Task<GetUserFeedsResponse> ExecuteAsync(CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();
        
        var subscriptions = await SubscriptionRepository.GetByUserIdAsync(userId, ct);
        
        var feeds = await DbContext.Feeds.AsNoTracking()
            .Where(f => subscriptions.Any(s => s.FeedId == f.Id))
            .Select(f => new FeedDto(f.Id, f.Name))
            .ToListAsync(ct);
        
        return new GetUserFeedsResponse(feeds);
    }
}
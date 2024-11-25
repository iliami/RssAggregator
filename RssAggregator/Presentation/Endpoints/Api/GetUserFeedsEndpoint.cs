using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserFeedsEndpoint(IFeedRepository FeedRepository) : EndpointWithoutRequest<GetUserFeedsResponse>
{
    public override void Configure()
    {
        Get("api/feeds/me");
    }

    public override async Task<GetUserFeedsResponse> ExecuteAsync(CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();
        
        var feeds = await FeedRepository.GetByUserIdAsync(userId, ct);
        
        return new GetUserFeedsResponse(feeds);
    }
}
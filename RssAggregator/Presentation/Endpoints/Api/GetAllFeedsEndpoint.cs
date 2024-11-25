using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetAllFeedsEndpoint(IFeedRepository FeedRepository)
    : EndpointWithoutRequest<GetAllFeedsResponse>
{
    public override void Configure()
    {
        Get("api/feeds");
    }

    public override async Task<GetAllFeedsResponse> ExecuteAsync(CancellationToken ct)
    {
        var feeds = await FeedRepository.GetFeedsAsync(ct);

        return new GetAllFeedsResponse(feeds);
    }
}
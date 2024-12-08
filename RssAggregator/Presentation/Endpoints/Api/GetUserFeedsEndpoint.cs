using FastEndpoints;
using RssAggregator.Application;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Params;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetUserFeedsEndpoint(IFeedRepository FeedRepository) : Endpoint<GetUserFeedsRequest, GetUserFeedsResponse>
{
    public override void Configure()
    {
        Get("api/feeds/me");
    }

    public override async Task<GetUserFeedsResponse> ExecuteAsync(GetUserFeedsRequest req, CancellationToken ct)
    {
        var (userId, _) = User.ToIdEmailTuple();
        var paginationParams = new PaginationParams
        {
            Page = req.Page,
            PageSize = req.PageSize,
        };

        var sortingParams = new SortingParams
        {
            SortBy = req.SortBy,
            SortDirection = req.SortDirection,
        };

        var feeds = await FeedRepository.GetByUserIdAsync(userId, paginationParams, sortingParams, ct);

        return new GetUserFeedsResponse(feeds);
    }
}
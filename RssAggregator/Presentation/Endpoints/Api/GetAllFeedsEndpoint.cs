using FastEndpoints;
using RssAggregator.Application;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Params;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class GetAllFeedsEndpoint(IFeedRepository FeedRepository)
    : Endpoint<GetAllFeedsRequest, GetAllFeedsResponse>
{
    public override void Configure()
    {
        Get("api/feeds");
    }

    public override async Task<GetAllFeedsResponse> ExecuteAsync(GetAllFeedsRequest req, CancellationToken ct)
    {
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
        
        var feeds = await FeedRepository.GetFeedsAsync(paginationParams, sortingParams, ct);

        return new GetAllFeedsResponse(feeds);
    }
}
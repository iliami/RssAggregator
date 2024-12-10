using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;
using RssAggregator.Presentation.Extensions;

namespace RssAggregator.Presentation.Endpoints.Api;

public record GetUserFeedsRequest(
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.None);

public record GetUserFeedsResponse(PagedResult<FeedDto> Feeds);

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
            PageSize = req.PageSize
        };

        var sortingParams = new SortingParams
        {
            SortBy = req.SortBy,
            SortDirection = req.SortDirection
        };

        var feeds = await FeedRepository.GetByUserIdAsync(userId, paginationParams, sortingParams, ct);

        return new GetUserFeedsResponse(feeds);
    }
}
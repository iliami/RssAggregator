using RssAggregator.Application;

namespace RssAggregator.Presentation.Contracts.Requests.Api;

public record GetAllFeedsRequest(
    int Page = 1,
    int PageSize = 10,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.None);
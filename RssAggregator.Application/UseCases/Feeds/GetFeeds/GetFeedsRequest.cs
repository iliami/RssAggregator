using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public record GetFeedsRequest(
    PaginationParams PaginationParams,
    SortingParams SortingParams);
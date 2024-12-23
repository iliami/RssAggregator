using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public interface IGetFeedsStorage
{
    Task<PagedResult<FeedDto>> GetFeeds(PaginationParams paginationParams, SortingParams sortingParams, CancellationToken ct = default);
}
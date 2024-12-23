using RssAggregator.Application.Models.DTO;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public record GetFeedsResponse(PagedResult<FeedDto> Feeds);
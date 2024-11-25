using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetAllFeedsResponse(IEnumerable<FeedDto> Feeds);
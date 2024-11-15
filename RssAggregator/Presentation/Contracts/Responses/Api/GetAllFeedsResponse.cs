using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetAllFeedsResponse(List<FeedDto> Feeds);
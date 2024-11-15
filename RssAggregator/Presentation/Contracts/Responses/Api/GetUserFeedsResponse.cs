using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetUserFeedsResponse(List<FeedDto> Feeds);
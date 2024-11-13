using RssAggregator.Features.Api.GetAllFeedsEndpoint;

namespace RssAggregator.Features.Api.GetUserFeedsEndpoint;

public record GetUserFeedsResponse(List<FeedDto> Feeds);
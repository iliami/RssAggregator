using RssAggregator.Application.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetUserFeedsResponse(PagedResult<FeedDto> Feeds);
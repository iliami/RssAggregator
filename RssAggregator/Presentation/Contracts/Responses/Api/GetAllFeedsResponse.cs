using RssAggregator.Application.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetAllFeedsResponse(PagedResult<FeedDto> Feeds);
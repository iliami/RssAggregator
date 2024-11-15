using RssAggregator.Presentation.DTO;
using RssAggregator.Presentation.DTO.FeedDto;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetAllFeedsResponse(List<FeedDto> Feeds);
using RssAggregator.Presentation.DTO;
using RssAggregator.Presentation.DTO.PostDto;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetUserPostsResponse(List<PostShortDto> Posts);
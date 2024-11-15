using RssAggregator.Presentation.DTO.PostDto;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetFeedPostsResponse(List<PostShortDto> Posts);
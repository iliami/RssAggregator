using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetUserPostsResponse(List<PostDto> Posts);
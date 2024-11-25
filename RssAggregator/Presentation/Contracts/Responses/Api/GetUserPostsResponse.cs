using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetUserPostsResponse(IEnumerable<PostDto> Posts);
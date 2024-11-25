using RssAggregator.Presentation.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetFeedPostsResponse(IEnumerable<PostDto> Posts);
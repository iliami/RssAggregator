using RssAggregator.Application.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetFeedPostsResponse(PagedResult<PostDto> Posts);
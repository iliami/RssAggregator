using RssAggregator.Application.DTO;

namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetUserPostsResponse(PagedResult<PostDto> Posts);
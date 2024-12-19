using RssAggregator.Application.Models.DTO;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public record GetPostsResponse(PagedResult<PostDto> Posts);
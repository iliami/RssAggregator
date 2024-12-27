using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPosts;

public record GetPostsResponse(Post[] Posts);
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetUserPosts;

public record GetUserPostsResponse(Post[] Posts);
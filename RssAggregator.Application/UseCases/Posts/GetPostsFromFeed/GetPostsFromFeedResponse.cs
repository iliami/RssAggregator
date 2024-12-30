using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;

public record GetPostsFromFeedResponse(Post[] Posts);
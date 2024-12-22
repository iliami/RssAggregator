using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.AddPostsInFeed;

public record AddPostsInFeedRequest(IReadOnlyCollection<Post> Posts, Feed Feed);
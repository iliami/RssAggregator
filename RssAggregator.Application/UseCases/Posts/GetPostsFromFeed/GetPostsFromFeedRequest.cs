using RssAggregator.Application.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Posts.GetPostsFromFeed;

public record GetPostsFromFeedRequest(Guid FeedId, Specification<Post> Specification);
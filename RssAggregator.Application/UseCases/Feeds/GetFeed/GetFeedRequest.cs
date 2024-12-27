using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public record GetFeedRequest(Guid FeedId, Specification<Feed> Specification);
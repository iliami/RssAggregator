using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public record GetFeedRequest<TProjection>(Guid FeedId, Specification<Feed, TProjection> Specification) 
    where TProjection : class;
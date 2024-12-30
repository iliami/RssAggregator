using RssAggregator.Application.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public record GetFeedsRequest(Specification<Feed> Specification);
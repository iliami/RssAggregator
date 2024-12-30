using RssAggregator.Application.Specifications;
using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetUserFeeds;

public record GetUserFeedsRequest(Specification<Feed> Specification);
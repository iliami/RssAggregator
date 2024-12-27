using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public record GetFeedsResponse(Feed[] Feeds);
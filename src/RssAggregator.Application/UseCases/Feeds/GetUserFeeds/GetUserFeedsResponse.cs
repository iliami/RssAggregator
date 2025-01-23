using RssAggregator.Domain.Entities;

namespace RssAggregator.Application.UseCases.Feeds.GetUserFeeds;

public record GetUserFeedsResponse(Feed[] Feeds);
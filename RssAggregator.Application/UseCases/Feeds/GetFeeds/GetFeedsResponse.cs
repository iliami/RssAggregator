namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public record GetFeedsResponse<TProjection>(TProjection[] Feeds);
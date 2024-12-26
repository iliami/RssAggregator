namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public record GetFeedResponse<TProjection>(TProjection Feed)
    where TProjection : class;
namespace RssAggregator.Application.UseCases.Feeds.UpdateFeed;

public interface IUpdateFeedUseCase
{
    Task<UpdateFeedResponse> Handle(UpdateFeedRequest request, CancellationToken ct = default);
}
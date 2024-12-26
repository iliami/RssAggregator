namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public interface IGetFeedUseCase<TProjection>
where TProjection : class
{
    Task<GetFeedResponse<TProjection>> Handle(GetFeedRequest<TProjection> request, CancellationToken ct = default);
}
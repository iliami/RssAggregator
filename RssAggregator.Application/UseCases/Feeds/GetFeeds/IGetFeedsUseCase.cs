namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public interface IGetFeedsUseCase<TProjection> where TProjection: class
{
    Task<GetFeedsResponse<TProjection>> Handle(
        GetFeedsRequest<TProjection> request, 
        CancellationToken ct = default);
}
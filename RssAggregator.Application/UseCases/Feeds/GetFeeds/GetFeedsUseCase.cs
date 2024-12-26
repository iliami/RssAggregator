namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public class GetFeedsUseCase<TProjection>(IGetFeedsStorage storage) 
    : IGetFeedsUseCase<TProjection>
    where TProjection : class
{
    public async Task<GetFeedsResponse<TProjection>> Handle(
        GetFeedsRequest<TProjection> request, 
        CancellationToken ct = default)
    {
        // await validator.ValidateAndThrowAsync(request, ct);
        
        var posts = await storage.GetFeeds(request.Specification, ct);
        
        var response = new GetFeedsResponse<TProjection>(posts);

        return response;
    }
}
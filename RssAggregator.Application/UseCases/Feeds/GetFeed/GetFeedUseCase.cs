using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.GetFeed;

public class GetFeedUseCase(IGetFeedStorage storage, IValidator<GetFeedRequest> validator) : IGetFeedUseCase
{
    public async Task<GetFeedResponse> Handle(GetFeedRequest request, CancellationToken ct = default)
    {
        validator.ValidateAndThrow(request);
        
        var (success, feed) = await storage.TryGetFeed(request.FeedId, ct);

        if (!success)
        {
            throw new Exception($"Feed not found: {request.FeedId}"); // TODO
        }

        var response = new GetFeedResponse(feed);

        return response;
    }
}
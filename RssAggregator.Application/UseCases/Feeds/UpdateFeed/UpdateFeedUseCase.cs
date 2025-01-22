using FluentValidation;
using RssAggregator.Domain.Entities;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Feeds.UpdateFeed;

public class UpdateFeedUseCase(
    IUpdateFeedStorage storage,
    IValidator<UpdateFeedRequest> validator)
    : IUpdateFeedUseCase
{
    public async Task<UpdateFeedResponse> Handle(UpdateFeedRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var (success, feedId) = await storage.TryUpdateFeed(request.Feed, ct);

        if (!success)
        {
            throw new FeedNotFoundException(feedId);
        }

        return new UpdateFeedResponse(feedId);
    }
}
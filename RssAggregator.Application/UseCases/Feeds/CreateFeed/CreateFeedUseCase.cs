using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.CreateFeed;

public class CreateFeedUseCase(ICreateFeedStorage storage, IValidator<CreateFeedRequest> validator) : ICreateFeedUseCase
{
    public async Task<CreateFeedResponse> Handle(CreateFeedRequest request, CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var feedId = await storage.CreateFeed(
            request.Name,
            request.Url, ct);

        var response = new CreateFeedResponse(feedId);

        return response;
    }
}
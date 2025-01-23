using FluentValidation;
using RssAggregator.Application.Auth;
using RssAggregator.Domain.Exceptions;

namespace RssAggregator.Application.UseCases.Feeds.CreateFeed;

public class CreateFeedUseCase(
    ICreateFeedStorage storage, 
    IValidator<CreateFeedRequest> validator,
    IIdentityProvider identityProvider) : ICreateFeedUseCase
{
    public async Task<CreateFeedResponse> Handle(CreateFeedRequest request, CancellationToken ct = default)
    {
        if (!identityProvider.Current.IsAdminRole())
        {
            throw new ForbiddenException();
        }

        await validator.ValidateAndThrowAsync(request, ct);

        var feedId = await storage.CreateFeed(
            request.Name,
            request.Url, ct);

        var response = new CreateFeedResponse(feedId);

        return response;
    }
}
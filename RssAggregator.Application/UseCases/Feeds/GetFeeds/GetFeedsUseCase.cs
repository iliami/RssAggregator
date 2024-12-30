using FluentValidation;

namespace RssAggregator.Application.UseCases.Feeds.GetFeeds;

public class GetFeedsUseCase(IGetFeedsStorage storage, IValidator<GetFeedsRequest> validator) : IGetFeedsUseCase
{
    public async Task<GetFeedsResponse> Handle(
        GetFeedsRequest request,
        CancellationToken ct = default)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        var posts = await storage.GetFeeds(request.Specification, ct);

        var response = new GetFeedsResponse(posts);

        return response;
    }
}
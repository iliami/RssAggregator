using FastEndpoints;
using RssAggregator.Application.Abstractions.Repositories;

namespace RssAggregator.Presentation.Endpoints.Admin;

public record CreateFeedRequest(string Name, string Url);

public class CreateFeedEndpoint(IFeedRepository FeedRepository) : Endpoint<CreateFeedRequest>
{
    public override void Configure()
    {
        Post("admin/create-feed");
        Roles("admin");
    }

    public override async Task HandleAsync(CreateFeedRequest req, CancellationToken ct)
    {
        await FeedRepository.AddAsync(req.Name, req.Url, ct);
    }
}
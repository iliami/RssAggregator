using FastEndpoints;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class CheckHealthEndpoint : Endpoint<CheckHealthRequest, CheckHealthResponse>
{
    public override void Configure()
    {
        Get("/api/check-health");
        AllowAnonymous();
    }

    public override Task<CheckHealthResponse> ExecuteAsync(CheckHealthRequest req, CancellationToken ct)
    {
        return Task.FromResult(new CheckHealthResponse(req.Check.ToUpper()));
    }
}
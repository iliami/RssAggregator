using FastEndpoints;
using RssAggregator.Presentation.Contracts.Requests.Api;
using RssAggregator.Presentation.Contracts.Responses.Api;

namespace RssAggregator.Presentation.Endpoints.Api;

public class HealthEndpoint : Endpoint<HealthRequest, HealthResponse>
{
    public override void Configure()
    {
        Get("/api/health");
        AllowAnonymous();
    }

    public override Task<HealthResponse> ExecuteAsync(HealthRequest req, CancellationToken ct)
    {
        return Task.FromResult(new HealthResponse(req.Check.ToUpper()));
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
// using FastEndpoints;
//
// namespace RssAggregator.Presentation.Endpoints.Api;
//
// public record CheckHealthRequest(string Check);
//
// public record CheckHealthResponse(string AllCaps);
//
// public class CheckHealthEndpoint : Endpoint<CheckHealthRequest, CheckHealthResponse>
// {
//     public override void Configure()
//     {
//         Get("/api/check-health");
//         AllowAnonymous();
//     }
//
//     public override Task<CheckHealthResponse> ExecuteAsync(CheckHealthRequest req, CancellationToken ct)
//     {
//         return Task.FromResult(new CheckHealthResponse(req.Check.ToUpper()));
//     }
// }
namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetFeedResponse(Guid Id, string Name, string Description, string Url);
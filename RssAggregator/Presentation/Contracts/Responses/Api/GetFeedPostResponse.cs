namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetFeedPostResponse(Guid Id, string Title, string Description, string Category, DateTime PublishDate, string Url, Guid FeedId);
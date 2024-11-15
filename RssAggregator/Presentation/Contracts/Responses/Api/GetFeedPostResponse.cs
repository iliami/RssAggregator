namespace RssAggregator.Presentation.Contracts.Responses.Api;

public record GetFeedPostResponse(Guid Id, string Title, string Description, DateTime PublishDate, string Url, Guid FeedId);
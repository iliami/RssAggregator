namespace RssAggregator.Application.DTO;

public record PostDto(Guid Id, string Title, string Category, DateTime PublishDate, string Url, Guid FeedId);
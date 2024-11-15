namespace RssAggregator.Presentation.DTO;

public record PostDto(Guid Id, string Title, DateTime PublishDate, string Url, Guid FeedId);
namespace RssAggregator.Presentation.DTO.PostDto;

public record PostShortDto(Guid Id, string Title, DateTime PublishDate, string Url, Guid FeedId);
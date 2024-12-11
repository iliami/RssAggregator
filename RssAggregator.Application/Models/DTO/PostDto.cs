namespace RssAggregator.Application.Models.DTO;

public record PostDto(Guid Id, string Title, string Category, DateTime PublishDate, string Url, Guid FeedId);
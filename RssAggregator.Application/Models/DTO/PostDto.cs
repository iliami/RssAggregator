namespace RssAggregator.Application.Models.DTO;

public record PostDto(Guid Id, string Title, string Categories, DateTime PublishDate, string Url, Guid FeedId);
namespace RssAggregator.Application.DTO;

public record FeedDto(Guid Id, string Name, string Description, string Url, int Subscribers, int Posts);
namespace RssAggregator.Application.Models.DTO;

public record FeedDto(Guid Id, string Name, string Description, string Url, int Subscribers, int Posts);
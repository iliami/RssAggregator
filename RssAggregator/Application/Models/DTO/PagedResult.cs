namespace RssAggregator.Application.Models.DTO;

public record PagedResult<T>(T[] Array, int Total);
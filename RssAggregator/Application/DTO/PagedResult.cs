namespace RssAggregator.Application.DTO;

public record PagedResult<T>(T[] Array, int Total);
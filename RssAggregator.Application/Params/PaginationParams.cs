using System.ComponentModel;

namespace RssAggregator.Application.Params;

public record PaginationParams(
    [property: DefaultValue(1)] int Page = 1,
    [property: DefaultValue(10)] int PageSize = 10);
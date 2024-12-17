using System.ComponentModel;

namespace RssAggregator.Application.Models.Params;

public class PaginationParams
{
    [DefaultValue(1)] public int Page { get; set; } = 1;
    [DefaultValue(10)] public int PageSize { get; set; } = 10;
}
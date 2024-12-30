using System.ComponentModel;

namespace RssAggregator.Application.Params;

public record SortingParams(
    string? SortBy = "",
    [property: DefaultValue("None")] SortDirection SortDirection = SortDirection.None);

public enum SortDirection
{
    None,
    Asc,
    Desc
}
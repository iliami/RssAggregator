using System.ComponentModel;

namespace RssAggregator.Application.Models.Params;

public class SortingParams
{
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.None;
}

public enum SortDirection
{
    [Description("none")] None,
    [Description("asc")] Asc,
    [Description("desc")] Desc
}
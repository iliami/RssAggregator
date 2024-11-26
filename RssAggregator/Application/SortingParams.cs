using System.ComponentModel;

namespace RssAggregator.Application;

public class SortingParams
{
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}

public enum SortDirection
{
    [Description("none")] None,
    [Description("asc")] Asc,
    [Description("desc")] Desc
}
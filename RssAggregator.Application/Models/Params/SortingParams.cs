using System.ComponentModel;

namespace RssAggregator.Application.Models.Params;

public class SortingParams
{
    public string? SortBy { get; set; }
    [DefaultValue("None")] public SortDirection SortDirection { get; set; } = SortDirection.None;
}

public enum SortDirection
{
    None,
    Asc,
    Desc
}
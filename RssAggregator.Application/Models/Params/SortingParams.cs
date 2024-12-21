using System.ComponentModel;

namespace RssAggregator.Application.Models.Params;

public class SortingParams
{
    [DefaultValue("")] public string SortBy { get; set; } = string.Empty;
    [DefaultValue("None")] public SortDirection SortDirection { get; set; } = SortDirection.None;
}

public enum SortDirection
{
    None,
    Asc,
    Desc
}
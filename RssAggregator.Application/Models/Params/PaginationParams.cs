namespace RssAggregator.Application.Models.Params;

public class PaginationParams
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
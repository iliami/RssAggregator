using RssAggregator.Application;
using RssAggregator.Application.Abstractions.KeySelectors;

namespace RssAggregator.Persistence.QueryExtensions;

public static class CommonQueryExtensions
{
    public static IQueryable<T> WithPagination<T>(this IQueryable<T> query, PaginationParams? paginationParams)
    {
        if (paginationParams == null) return query;
        
        var skip = (paginationParams.Page - 1) * paginationParams.PageSize;
        var take = paginationParams.PageSize;
        
        return query.Skip(skip).Take(take);
    }
    
    public static IQueryable<T> WithSorting<T>(this IQueryable<T> query, SortingParams? sortParams, IKeySelector<T> keySelector)
    {
        if (sortParams is null ||
            sortParams.SortDirection == SortDirection.None)
        {
            return query;
        }

        var orderBySelector = keySelector.GetKeySelector(sortParams.SortBy);

        return sortParams.SortDirection == SortDirection.Asc
            ? query.OrderBy(orderBySelector)
            : query.OrderByDescending(orderBySelector);
    }
}
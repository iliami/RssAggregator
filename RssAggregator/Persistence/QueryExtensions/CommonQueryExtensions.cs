using RssAggregator.Application;
using RssAggregator.Domain.Entities;

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
}
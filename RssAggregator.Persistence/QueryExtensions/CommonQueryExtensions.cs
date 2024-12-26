using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Abstractions.KeySelectors;
using RssAggregator.Application.Abstractions.Specifications;
using RssAggregator.Application.Models.DTO;
using RssAggregator.Application.Models.Params;

namespace RssAggregator.Persistence.QueryExtensions;

public static class CommonQueryExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PaginationParams paginationParams,
        CancellationToken ct = default)
    {
        var count = await query.CountAsync(ct);
        if (count == 0) return new PagedResult<T>([], 0);

        var skip = (paginationParams.Page - 1) * paginationParams.PageSize;
        var take = paginationParams.PageSize;

        var result = await query.Skip(skip).Take(take).ToArrayAsync(ct);

        return new PagedResult<T>(result, count);
    }

    public static IQueryable<T> WithSorting<T>(
        this IQueryable<T> query,
        SortingParams sortParams,
        IKeySelector<T> keySelector)
    {
        if (sortParams.SortBy == string.Empty || sortParams.SortDirection == SortDirection.None)
            return query;

        var orderBySelector = keySelector.GetKeySelector(sortParams.SortBy);

        return sortParams.SortDirection == SortDirection.Asc
            ? query.OrderBy(orderBySelector)
            : query.OrderByDescending(orderBySelector);
    }
    public static IQueryable<TResult> EvaluateSpecification<TEntity, TResult>(
        this IQueryable<TEntity> query,
        Specification<TEntity, TResult> specification)
        where TEntity : class
        where TResult : class
    {
        if (specification.IsNoTracking)
        {
            query = query.AsNoTracking();
        }
        
        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }
        
        if (specification.OrderBy is not null)
        {
            query = specification.IsAscendingOrderBy ? query.OrderBy(specification.OrderBy) : query.OrderByDescending(specification.OrderBy);
        }
        
        if (specification.Skip <= 0)
        {
            query = query.Skip(specification.Skip);
        }

        if (specification.Take != int.MaxValue && specification.Take >= 0)
        {
            query = query.Take(specification.Take);
        }
        
        query = specification.Includes.Aggregate(
            query,
            (current, include) => current.Include(include));

        return query.Select(specification.Projection);
    }
}
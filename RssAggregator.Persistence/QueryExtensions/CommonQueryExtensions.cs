using Microsoft.EntityFrameworkCore;
using RssAggregator.Application.Specifications;

namespace RssAggregator.Persistence.QueryExtensions;

public static class CommonQueryExtensions
{
    public static IQueryable<TEntity> EvaluateSpecification<TEntity>(
        this IQueryable<TEntity> query,
        Specification<TEntity> specification)
        where TEntity : class
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
            query = specification.IsAscendingOrderBy
                ? query.OrderBy(specification.OrderBy)
                : query.OrderByDescending(specification.OrderBy);
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

        return query;
    }
}
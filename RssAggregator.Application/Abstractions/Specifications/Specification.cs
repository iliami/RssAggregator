using System.Linq.Expressions;

namespace RssAggregator.Application.Abstractions.Specifications;

public abstract class Specification<TEntity>
    where TEntity : class
{
    public bool IsNoTracking { get; protected init; } = false;
    
    public int Skip { get; protected init; } = 0;
    public int Take { get; protected init; } = int.MaxValue;

    public Expression<Func<TEntity, bool>>? Criteria { get; protected init; }
    
    public Expression<Func<TEntity, object>>? OrderBy { get; private set; }
    public bool IsAscendingOrderBy { get; private set; } = true;
    protected void SetAscendingOrderBy(Expression<Func<TEntity, object>> orderBy) 
        => (OrderBy, IsAscendingOrderBy) = (orderBy, true);
    protected void SetDescendingOrderBy(Expression<Func<TEntity, object>> orderBy) 
        => (OrderBy, IsAscendingOrderBy) = (orderBy, false);
    
    public List<Expression<Func<TEntity, object>>> Includes { get; } = [];
    protected void AddInclude(Expression<Func<TEntity, object>> include) 
        => Includes.Add(include);
}
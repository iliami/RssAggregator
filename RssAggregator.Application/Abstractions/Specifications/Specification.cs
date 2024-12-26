using System.Linq.Expressions;

namespace RssAggregator.Application.Abstractions.Specifications;

public abstract class Specification<TEntity>() 
    : Specification<TEntity, TEntity>(obj => obj)
    where TEntity : class;

public abstract class Specification<TEntity, TResult>(Expression<Func<TEntity, TResult>> projection)
    where TEntity : class
    where TResult : class
{
    public bool IsNoTracking { get; protected init; } = false;
    
    public int Skip { get; protected init; } = 0;
    public int Take { get; protected init; } = int.MaxValue;
    
    public Expression<Func<TEntity, TResult>> Projection { get; } = projection;

    private Expression<Func<TEntity, bool>>? _criteria;

    public Expression<Func<TEntity, bool>>? Criteria
    {
        get => _criteria;
        protected set => _criteria = value ?? throw new ArgumentNullException(nameof(value));
    }
    
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
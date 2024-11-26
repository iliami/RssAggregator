using System.Linq.Expressions;

namespace RssAggregator.Application.Abstractions.KeySelectors;

public interface IKeySelector<T>
{
    public Expression<Func<T, object>> GetKeySelector(string? param);
}
using System.Linq.Expressions;

namespace RssAggregator.Application.KeySelectors;

public interface IKeySelector<T>
{
    public Expression<Func<T, object>> GetKeySelector(string? fieldName);
}
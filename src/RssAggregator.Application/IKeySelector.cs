using System.Linq.Expressions;

namespace RssAggregator.Application;

public interface IKeySelector<T>
{
    public Expression<Func<T, object>> GetKeySelector(string? fieldName);
}
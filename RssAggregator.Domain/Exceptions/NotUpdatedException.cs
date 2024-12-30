namespace RssAggregator.Domain.Exceptions;

public class NotUpdatedException<T>(Guid id)
    : Exception($"Failed to update the {DomainModelType} with the id \"{id}\".")
{
    private static Type DomainModelType { get; } = typeof(T);
}
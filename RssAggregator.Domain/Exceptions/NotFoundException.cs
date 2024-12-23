namespace RssAggregator.Domain.Exceptions;

public class NotFoundException<T>(Guid id)
    : Exception($"The {DomainModelType} with the id \"{id}\" could not be found.")
{
    private static Type DomainModelType { get; } = typeof(T);
}
namespace RssAggregator.Domain.Exceptions;

public interface INotFoundException
{
    string Message { get; }
}
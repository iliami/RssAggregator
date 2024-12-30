namespace RssAggregator.Presentation.Services;

public interface IHashCreator
{
    string GetHash(string value);
}
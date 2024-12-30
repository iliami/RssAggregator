namespace RssAggregator.Presentation.Services;

public interface IHashComparer
{
    bool CompareWithHash(string hash, string valueToCompareWithHash);
}
namespace Iliami.Identity.Domain.HashingHelpers;

public interface IHashComparer
{
    bool CompareWithHash(string hash, string valueToCompareWithHash);
}
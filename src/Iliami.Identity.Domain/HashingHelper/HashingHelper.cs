namespace Iliami.Identity.Domain.HashingHelpers;

public class HashingHelper : IHashCreator, IHashComparer
{
    public string GetHash(string value)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(value);
    }

    public bool CompareWithHash(string hash, string valueToCompareWithHash)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(valueToCompareWithHash, hash);
    }
}
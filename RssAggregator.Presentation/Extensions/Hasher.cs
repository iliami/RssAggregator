namespace RssAggregator.Presentation.Extensions;

public static class Hasher
{
    public static string GetHash(this string stringToHash)
    {
        return BCrypt.Net.BCrypt.EnhancedHashPassword(stringToHash);
    }

    public static bool IsEqualToHashOf(this string hashedPassword, string password)
    {
        return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
    }
}
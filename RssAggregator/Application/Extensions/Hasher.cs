namespace RssAggregator.Application.Extensions;

public static class Hasher
{
    public static string GetHash(this string stringToHash)
        => BCrypt.Net.BCrypt.EnhancedHashPassword(stringToHash);

    public static bool IsEqualToHashOf(this string hashedPassword, string password)
        => BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
}
namespace Iliami.Identity.Domain.HashingHelpers;

public interface IHashCreator
{
    string GetHash(string value);
}
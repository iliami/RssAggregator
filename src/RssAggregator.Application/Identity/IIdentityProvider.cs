namespace RssAggregator.Application.Identity;

public interface IIdentityProvider
{
    IIdentity Current { get; set; }
}

public class IdentityProvider : IIdentityProvider
{
    public required IIdentity Current { get; set; }
}
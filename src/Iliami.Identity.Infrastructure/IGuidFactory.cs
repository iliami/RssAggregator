namespace Iliami.Identity.Infrastructure;

public interface IGuidFactory
{
    Guid CreateGuid();
}

public class GuidFactory : IGuidFactory
{
    public Guid CreateGuid()
        => Guid.NewGuid();
}
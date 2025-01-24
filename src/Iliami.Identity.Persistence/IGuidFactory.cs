namespace Iliami.Identity.Persistence;

public interface IGuidFactory
{
    Guid CreateGuid();
}

public class GuidFactory : IGuidFactory
{
    public Guid CreateGuid()
        => Guid.NewGuid();
}
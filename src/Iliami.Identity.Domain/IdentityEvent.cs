namespace Iliami.Identity.Domain;

public class IdentityEvent
{
    public Guid Id { get; set; }
    public DateTimeOffset EmittedAt { get; set; }
    public byte[] ContentBlob { get; set; } = [];
}
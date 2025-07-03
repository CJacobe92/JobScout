namespace JobScout.Domain.Outbox;

public class Outbox
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string EventType { get; init; } = default!;
    public string ContentJson { get; init; } = default!;
    public DateTime CreatedOn { get; init; } = DateTime.UtcNow;
    public DateTime? ProcessedOn { get; set; }
}
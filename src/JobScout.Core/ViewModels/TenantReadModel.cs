using JobScout.Domain.Enumerations;

namespace JobScout.Core.ViewModels;

public class TenantReadModel
{
    public Guid Id { get; set; }
    public string CompanyName { get; set; } = default!;
    public AvailableShards ShardKey { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

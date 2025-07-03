namespace JobScout.Infrastructure.Database;

public class TenantMongoDatabaseSettings
{
    public string ConnectionString { get; set; } = default!;
    public string DatabaseName { get; set; } = default!;
}
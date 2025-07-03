using System;
using System.Text.Json.Serialization;
using JobScout.Domain.SeedWork;

namespace JobScout.Domain.Tenants;

public class TenantCreatedEvent : DomainEventBase
{
    [JsonIgnore]
    public TenantId TenantId { get; set; }

    [JsonPropertyName("TenantId")]
    public string TenantIdRaw
    {
        get => TenantId.Value.ToString("D");
        set => TenantId = new TenantId(Guid.Parse(value));
    }

    public string CompanyName { get; set; }

    public TenantCreatedEvent() { }

    public TenantCreatedEvent(TenantId tenantId, string companyName)
    {
        TenantId = tenantId;
        CompanyName = companyName;
    }
}

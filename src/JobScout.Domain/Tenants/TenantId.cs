using System;
using System.Text.Json.Serialization;

namespace JobScout.Domain.Tenants
{
    public readonly record struct TenantId(Guid Value)
    {
        public static TenantId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString("D");

        public static bool TryParse(string input, out TenantId tenantId)
        {
            if (Guid.TryParse(input, out var guid))
            {
                tenantId = new TenantId(guid);
                return true;
            }

            tenantId = default;
            return false;
        }
    }
}

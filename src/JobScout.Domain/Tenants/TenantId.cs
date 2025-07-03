using System;
using System.Text.Json.Serialization;

namespace JobScout.Domain.Tenants
{
    public readonly record struct TenantId(Guid Value)
    {
        public static TenantId New() => new(Guid.NewGuid());

        public override string ToString() => Value.ToString("D");

        public static TenantId Parse(string raw) =>
            new(Guid.TryParse(raw, out var g) ? g : throw new FormatException("Invalid GUID"));
    }
}

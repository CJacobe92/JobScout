using JobScout.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Tenants
{
    public record struct TenantId(Guid Value)
    {
        public static TenantId New() => new(Guid.NewGuid());
        public override readonly string ToString() => Value.ToString();
    }
}

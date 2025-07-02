using JobScout.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Tenants
{
    public class TenantCreatedEvent(TenantId _tenantId, string _companyName) : DomainEventBase
    {
        public TenantId TenantId { get; } = _tenantId;
        public string CompanyName { get; } = _companyName;
    }
}

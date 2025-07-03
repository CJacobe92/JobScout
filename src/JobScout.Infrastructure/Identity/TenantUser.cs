using JobScout.Domain.Tenants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Infrastructure.Identity
{
    public class TenantUser : IdentityUser<Guid>
    {
        public string FirstName { get; init; } = default!;
        public string LastName { get; init; } = default!;
        public string? RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiry { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;
        public TenantId TenantId { get; init; } = default!;
        public TenantUser() { }

        private TenantUser(string firstName, string lastName, string email, TenantId tenantId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UserName = email;
            this.TenantId = tenantId;
        }

        public static TenantUser Create(string firstName, string lastName, string email, TenantId tenantId)
        {
            return new TenantUser(firstName, lastName, email, tenantId);
        }
    }

}

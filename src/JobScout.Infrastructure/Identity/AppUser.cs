using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Infrastructure.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public string? RefreshToken { get; init; }
        public DateTime? RefreshTokenExpiry { get; init; }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; init; } = DateTime.UtcNow;

    }
}

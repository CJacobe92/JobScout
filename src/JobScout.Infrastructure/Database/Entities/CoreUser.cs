using Microsoft.AspNetCore.Identity;
using JobScout.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace JobScout.Infrastructure.Database.Entities
{
    public class CoreUser : IdentityUser<Guid>, IAuditableEntity
    {
        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}

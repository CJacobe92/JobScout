using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace JobScout.Core.Commands.Tenant
{
    public record CreateTenantCommand : IRequest<Guid>
    {
        public required string CompanyName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

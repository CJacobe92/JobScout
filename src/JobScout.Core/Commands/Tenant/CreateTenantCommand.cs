using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScout.Domain.Contracts;
using MediatR;

namespace JobScout.Core.Commands.Tenant;

public class CreateTenantCommand(
    string companyName,
    string firstName,
    string lastName,
    string email,
    string password
) : IRequest<Guid>
{
    public string CompanyName { get; set; } = companyName;
    public string FirstName { get; set; } = firstName;
    public string LastName { get; set; } = lastName;
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}

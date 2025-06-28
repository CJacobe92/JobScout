using System;
using System.ComponentModel.DataAnnotations;
using JobScout.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace JobScout.Infrastructure.Database.Entities;

public class TenantUser : IdentityUser<Guid>, IAuditableEntity
{
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required override string Email { get; set; }
  public required override string UserName { get; set; }
  public required Guid TenantId { get; set; }
  public string? RefreshToken { get; set; }
  public DateTime? RefreshTokenExpiry { get; set; }
  public DateTime? CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }

}

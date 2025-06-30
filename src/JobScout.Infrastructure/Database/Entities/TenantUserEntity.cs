using System;
using System.ComponentModel.DataAnnotations;
using JobScout.Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace JobScout.Infrastructure.Database.Entities;

public class TenantUserEntity : IdentityUser<Guid>, IAuditableEntity
{
  public required string FirstName { get; set; } = default!;
  public required string LastName { get; set; } = default!;
  public required override string? Email { get; set; } = default!;
  public required override string? UserName { get; set; } = default!;
  public required Guid TenantId { get; set; } = default!;
  public string? RefreshToken { get; set; } = default!;
  public DateTime? RefreshTokenExpiry { get; set; } = default!;
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  // public TenantUserEntity(string firstName, string lastName, string email, Guid tenantId)
  // {
  //   FirstName = firstName;
  //   LastName = lastName;
  //   Email = email;
  //   UserName = email;
  //   TenantId = tenantId;
  // }

  // public TenantUserEntity() { }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace JobScout.API.GraphQL.Tenants.Input;

public record CreateTenantInput(
    [Required]
    [GraphQLNonNullType] string CompanyName,

    [Required]
    [GraphQLNonNullType] string FirstName,

    [Required]
    [GraphQLNonNullType] string LastName,

    [Required]
    [EmailAddress] string Email,

    [Required]
    [MinLength(8)] string Password
);
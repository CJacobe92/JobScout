using System;
using System.ComponentModel.DataAnnotations;

namespace JobScout.API.GraphQL.Tenants.Input;

public record UpdateTenantInput(
    string? CompanyName,
    bool? IsActivated,
    bool? WelcomeEmailSent
);
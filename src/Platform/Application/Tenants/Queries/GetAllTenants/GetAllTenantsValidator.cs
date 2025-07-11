using Application.Tenants.Queries.GetAllTenants;
using FluentValidation;

namespace Application.Tenants.Validators.GetAllTenants;

public class GetAllTenantsValidator : AbstractValidator<GetAllTenantsQuery>
{
    private const int MaxPageSize = 100;
    private static readonly string[] AllowedFields = ["name", "registeredto", "license"];

    public GetAllTenantsValidator()
    {
        RuleFor(x => x.Search)
            .MaximumLength(100)
            .Matches(@"^(?!.*(\bSELECT\b|\bDROP\b|\bINSERT\b|\bDELETE\b|\bUPDATE\b|\*|;|--)).*$")
            .WithMessage("Search query contains potentially unsafe characters or SQL keywords.");

        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, MaxPageSize)
            .WithMessage($"PageSize must be between 1 and {MaxPageSize}.");

        RuleFor(x => x.By)
            .NotEmpty()
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage("'By' must be provided when using 'Search'.");

        RuleFor(x => x.By)
            .Must(field => AllowedFields.Contains(field?.ToLowerInvariant() ?? ""))
            .When(x => !string.IsNullOrWhiteSpace(x.By))
            .WithMessage($"'By' must be one of the following: {string.Join(", ", AllowedFields)}.");
    }
}

using FluentValidation;

namespace Application.Tenants.Validators.GetAllTenants;

public class GetAllTenantsDtoValidator : AbstractValidator<GetAllTenantsDto>
{
    private const int MaxPageSize = 100;

    public GetAllTenantsDtoValidator()
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
    }
}

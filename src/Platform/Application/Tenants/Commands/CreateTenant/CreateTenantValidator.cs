using System;
using Application.Tenants.Commands.CreateTenant;
using FluentValidation;

namespace Application.Tenants.Validators.CreateTenant;

public class CreateTenantCommandValidator : AbstractValidator<CreateTenantCommand>
{
    public CreateTenantCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tenant name is required.")
            .MaximumLength(100).WithMessage("Tenant name cannot exceed 100 characters.");

        RuleFor(x => x.License)
            .NotEmpty().WithMessage("License information is required.")
            .MaximumLength(50).WithMessage("License cannot exceed 50 characters.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[0-9]{11,13}$").WithMessage("Invalid phone number format."); // Basic regex for 10-15 digits, optional '+'

        RuleFor(x => x.RegisteredTo)
            .NotEmpty().WithMessage("Registered user information is required.")
            // .EmailAddress().WithMessage("Registered user must be a valid email address.")
            .MaximumLength(200).WithMessage("Registered user email cannot exceed 200 characters.");

        RuleFor(x => x.TIN)
            .NotEmpty().WithMessage("TIN is required.")
            .Length(9, 12).WithMessage("TIN must be between 9 and 12 characters."); // Example: Adjust based on actual TIN format

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");
    }
}
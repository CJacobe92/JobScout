using Infrastructure.Persistence;
using Application.Tenants.Validators.CreateTenant;
using FluentValidation;
using Application.Tenants.Commands.CreateTenant;
using Application.Tenants.Validators.GetAllTenants;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Application.Tenants.Queries.GetAllTenants;


namespace Api;

public static class TenantEndpoints
{
    public static void MapTenantEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tenants", async (
            [FromBody] CreateTenantDto request,
            [FromServices] IValidator<CreateTenantDto> validator, // <--- Inject the validator
            [FromServices] CreateTenantHandler handler,
            CancellationToken ct
            ) =>
        {
            // Perform manual validation
            ValidationResult validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var command = new CreateTenantCommand(
                request.Name,
                request.License,
                request.Phone,
                request.RegisteredTo,
                request.TIN,
                request.Address
            );

            var tenant = await handler.HandleAsync(command, ct);
            return Results.Created($"/tenants/{tenant}", new { Data = tenant });
        })
        .WithName("CreateTenant")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest); //


        app.MapGet("/api/tenants", async (
            [AsParameters] GetAllTenantsDto request,
            [FromServices] IValidator<GetAllTenantsDto> validator,
            [FromServices] GetAllTenantsHandler handler,
            CancellationToken ct
        ) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(request, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var command = new GetAllTenantsQuery(
                request.Search,
                request.By,
                request.Page,
                request.PageSize
            );

            var tenants = await handler.HandleAsync(command, ct);
            return Results.Ok(new { Data = tenants }); // 👈 Standardized response format
        })
        .WithName("GetAllTenants")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
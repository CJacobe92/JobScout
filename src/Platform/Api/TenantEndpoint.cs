using Infrastructure.Persistence;
using Application.Tenants.Validators.CreateTenant;
using FluentValidation;
using Application.Tenants.Commands.CreateTenant;
using Application.Tenants.Validators.GetAllTenants;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Application.Tenants.Queries.GetAllTenants;
using MediatR;


namespace Api;

public static class TenantEndpoints
{
    public static void MapTenantEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tenants", async (
            [FromBody] CreateTenantCommand command,
            [FromServices] IValidator<CreateTenantCommand> validator,
            [FromServices] IMediator mediator,
            CancellationToken ct
            ) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(command, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var tenant = await mediator.Send(command, ct);
            return Results.Created($"/tenants/{tenant}", new { Data = tenant });
        })
        .WithName("CreateTenant")
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest); //


        app.MapGet("/api/tenants", async (
            [AsParameters] GetAllTenantsQuery query,
            [FromServices] IValidator<GetAllTenantsQuery> validator,
            [FromServices] IMediator mediator,
            CancellationToken ct
        ) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(query, ct);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var tenants = await mediator.Send(query, ct);
            return Results.Ok(new { Data = tenants }); // 👈 Standardized response format
        })
        .WithName("GetAllTenants")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
using Infrastructure.Persistence;
using Application.Commands.CreateTenant;
using Application.Tenants.Validators.CreateTenant;
using FluentValidation;
using FluentValidation.Results;
using Application.Tenants.Commands.CreateTenant;


namespace Api;

public static class TenantEndpoints
{
    public static void MapTenantEndpoints(this WebApplication app)
    {
        app.MapPost("/api/tenants", async (
            CreateTenantDto request,
            IValidator<CreateTenantDto> validator, // <--- Inject the validator
            CreateTenantHandler handler,
            CancellationToken ct) =>
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


        // app.MapGet("/api/tenants", async (
        //     CancellationToken ct,
        //     string? search,
        //     int page = 1,
        //     int pageSize = 10) =>
        // {


        // });

    }
}
using System;
using JobScout.API.GraphQL.Tenants.Input;
using JobScout.API.GraphQL.Tenants.Output;
using JobScout.Application.Tenants.Commands;
using MediatR;

namespace JobScout.API.GraphQL.Tenants;

public class TenantMutations
{
    public async Task<TenantOutput> CreateTenant(
    CreateTenantInput input,
    [Service] IMediator mediator,
    CancellationToken ct
)
    {
        ct.ThrowIfCancellationRequested();
        var command = new CreateTenantCommand(
            input.CompanyName,
            input.FirstName,
            input.LastName,
            input.Email,
            input.Password,
            ct
        );

        var result = await mediator.Send(command, ct);

        if (!result.IsSuccess)
        {
            var errors = result.Errors
            .Select(e => ErrorBuilder.New()
                .SetMessage(e)
                .Build())
            .ToList();
            throw new GraphQLException(errors);
        }

        return new TenantOutput(
            result.Value!.Id.Value.ToString(),
            result.Value.CompanyName,
            result.Value.CreatedAt.ToString("O"),
            result.Value.UpdatedAt.ToString("O")
        );
    }

    public async Task<TenantOutput> UpdateTenant(
        UpdateTenantInput input,
        [Service] IMediator mediator,
        CancellationToken ct
    )
    {
        ct.ThrowIfCancellationRequested();

        var command = new UpdateTenantCommand(input.CompanyName, ct);

        var result = await mediator.Send(command, ct);

        if (!result.IsSuccess)
        {
            var errors = result.Errors
            .Select(e => ErrorBuilder.New()
                .SetMessage(e)
                .Build())
            .ToList();
            throw new GraphQLException(errors);
        }

        return new TenantOutput(
            result.Value!.Id.Value.ToString(),
            result.Value.CompanyName,
            result.Value.CreatedAt.ToString("O"),
            result.Value.UpdatedAt.ToString("O")
        );
    }
}



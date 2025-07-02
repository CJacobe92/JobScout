using System;
using JobScout.API.GraphQL.Tenants.Input;
using JobScout.API.GraphQL.Tenants.Output;
using JobScout.Application.Tenants.Commands;
using JobScout.Domain.Tenants;
using MediatR;

namespace JobScout.API.GraphQL.Tenants;

public class TenantMutations
{
    public async Task<CreateTenantOutput> CreateTenant(
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
            input.Password
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

        return new CreateTenantOutput(
            result.Value!.Id.ToString(),
            result.Value.CompanyName,
            result.Value.CreatedAt.ToString("O"),
            result.Value.UpdatedAt.ToString("O")
        );
    }
}



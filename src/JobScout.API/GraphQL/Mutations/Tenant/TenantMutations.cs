using JobScout.API.GraphQL.Inputs.Tenant;
using JobScout.Core.Commands.Tenant;
using JobScout.Domain.Enumerations;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.API.GraphQL.Mutations.Tenant;

public class TenantMutations
{
    public async Task<Guid> CreateTenant(
    CreateTenantInput input,
    [Service] IMediator mediator,
    CancellationToken ct)
    {
        var command = new CreateTenantCommand(
            input.CompanyName,
            input.FirstName,
            input.LastName,
            input.Email,
            input.Password);

        return await mediator.Send(command, ct);
    }

    public async Task<TenantModel> UpdateTenant(
    UpdateTenantInput input,
    [Service] IMediator mediator,
    CancellationToken ct
    )
    {
        AvailableShards? parsedShard = null;

        if (!string.IsNullOrWhiteSpace(input.Shard))
        {
            if (Enum.TryParse<AvailableShards>(input.Shard, ignoreCase: true, out var result))
            {
                parsedShard = result;
            }
            else
            {
                throw new GraphQLException($"Invalid shard key: {input.Shard}");
            }
        }
        var command = new UpdateTenantCommand(
            input.Id,
            input.CompanyName,
            parsedShard
        );

        return await mediator.Send(command, ct);
    }

    public async Task<Guid> DeleteTenant(
    DeleteTenantInput input,
    [Service] IMediator mediator,
    CancellationToken ct
    )
    {
        var command = new DeleteTenantCommand
        {
            Id = input.Id
        };

        return await mediator.Send(command, ct);
    }

}
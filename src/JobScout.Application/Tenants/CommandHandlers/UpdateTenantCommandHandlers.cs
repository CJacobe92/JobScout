using JobScout.Application.Tenants.Commands;
using JobScout.Domain.SeedWork;
using JobScout.Domain.Tenants;
using JobScout.Domain.Tenants.Contracts;
using JobScout.Domain.Tenants.Rules;
using MediatR;

namespace JobScout.Application.Tenants.CommandHandlers;

public class UpdateTenantCommandHandler(
    ITenantWriteRepository repo,
    IUniquenessChecker<string> uniquenessChecker
) :
IRequestHandler<UpdateTenantCommand, IResult<Tenant?>>
{
    public async Task<IResult<Tenant?>> Handle(UpdateTenantCommand command, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var rule = new CompanyNameMustBeUniqueRule(command.CompanyName, uniquenessChecker);
        if (await rule.IsBrokenAsync())
            return Result<Tenant?>.Failure(rule.Message);

        ct.ThrowIfCancellationRequested();

        var result = await repo.UpdateTenantAsync(
            command.CompanyName,
            ct
        );

        return result;
    }

}
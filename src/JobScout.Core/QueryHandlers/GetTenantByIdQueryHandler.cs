using System;
using AutoMapper;
using JobScout.Core.Exceptions;
using JobScout.Core.Queries.Tenant;
using JobScout.Core.ViewModels;
using JobScout.Domain.Contracts;
using MediatR;

namespace JobScout.Core.QueryHandlers;

public class GetTenantByIdQueryHandler(IMapper mapper, IUnitOfWork uow, ITenantRepository repo)
: IRequestHandler<GetTenantByIdQuery, TenantViewModel>
{
    public async Task<TenantViewModel> Handle(GetTenantByIdQuery query, CancellationToken ct)
    {
        await uow.BeginTransactionAsync(ct);

        try
        {
            var tenant = await repo.GetOneById(query.Id, ct)
                ?? throw new NotFoundException($"Tenant with ID: {query.Id} not found");

            await uow.CommitAsync(ct);

            return mapper.Map<TenantViewModel>(tenant);
        }
        catch (System.Exception)
        {
            await uow.RollbackAsync(ct);
            throw;
        }
    }
}
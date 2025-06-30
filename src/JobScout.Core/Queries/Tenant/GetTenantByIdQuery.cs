using System;
using JobScout.Core.ViewModels;
using MediatR;

namespace JobScout.Core.Queries.Tenant;

public record GetTenantByIdQuery : IRequest<TenantViewModel>
{
    public Guid Id { get; set; }

}

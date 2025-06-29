using System;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.Queries.Tenant;

public class GetTenantById : IRequest<TenantModel>
{

}

using System;
using JobScout.Core.ViewModels;
using MediatR;

namespace JobScout.Core.Queries.Tenant;

public class GetAllTenants : IRequest<IEnumerable<TenantViewModel>>;

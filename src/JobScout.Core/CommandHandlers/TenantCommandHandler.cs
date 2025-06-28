using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobScout.Core.Commands.Tenant;
using JobScout.Domain.Contracts;
using TenantUserEntity = JobScout.Infrastructure.Database.Entities.TenantUser;
using TenantUserModel = JobScout.Domain.Models.TenantUser;
using MediatR;
using Microsoft.AspNetCore.Identity;
using JobScout.Domain.Models;
using JobScout.Infrastructure.Database.Mappings;

namespace JobScout.Core.CommandHandlers
{
    public class TenantCommandHandler(
        IMapper mapper,
        ITenantRepository tenantRepository,
        UserManager<TenantUserEntity> userManager
    ) : IRequestHandler<CreateTenantCommand, Guid>
    {

        private readonly IMapper _mapper = mapper;
        private readonly ITenantRepository _repo = tenantRepository;

        public async Task<Guid> Handle(CreateTenantCommand command, CancellationToken ct)
        {
            var tenant = _mapper.Map<Tenant>(command);

            var user = new TenantUserModel(
                command.FirstName,
                command.LastName,
                command.Email,
                command.Email,
                tenant.Id
            );

            var userEntity = user.ToEntity();
            await userManager.CreateAsync(userEntity, command.Password);

            return await _repo.CreateTenant(tenant);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobScout.Core.Commands.Tenant;
using JobScout.Domain.Contracts;
using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.CommandHandlers
{
    public class TenantCommandHandler(IMapper mapper, ITenantRepository tenantRepository) : IRequestHandler<CreateTenantCommand, Guid>
    {

        private readonly IMapper _mapper = mapper;
        private readonly ITenantRepository _repo = tenantRepository;
        
        public async Task<Guid> Handle(CreateTenantCommand command, CancellationToken ct)
        {
            var tenant = _mapper.Map<Tenant>(command);
            return await _repo.CreateTenant(tenant);
        }
    }
    


}

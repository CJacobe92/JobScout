using Microsoft.AspNetCore.Mvc;
using JobScout.AppService.DTO;


using MediatR;
using AutoMapper;
using JobScout.API.Common;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.ViewModels;

namespace JobScout.API.Controllers
{
    public class TenantController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)

    {
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto dto)
        {
            return await Handle<CreateTenantDto, CreateTenantCommand, Guid>(dto);
        }
    }
}

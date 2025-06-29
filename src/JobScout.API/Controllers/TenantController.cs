using Microsoft.AspNetCore.Mvc;
using JobScout.App.DTO;
using MediatR;
using AutoMapper;
using JobScout.API.Common;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.ViewModels;
using JobScout.Domain.Models;

namespace JobScout.API.Controllers
{
    public class TenantController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)

    {
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto dto)
        {
            return await Handle<CreateTenantDto, CreateTenantCommand, Guid>(null, dto);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateTenant(Guid id, [FromBody] UpdateTenantDto dto)
        {
            return await Handle<UpdateTenantDto, UpdateTenantCommand, TenantModel>(id, dto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTenant(Guid id)
        {
            return await Handle<DeleteTenantCommand, Guid>(id);
        }
    }
}

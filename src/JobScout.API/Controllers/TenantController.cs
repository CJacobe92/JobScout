using Microsoft.AspNetCore.Mvc;
using JobScout.App.DTO;
using MediatR;
using AutoMapper;
using JobScout.API.Common;
using JobScout.Core.Commands.Tenant;
using JobScout.Core.ViewModels;
using JobScout.Domain.Models;
using JobScout.Core.Queries.Tenant;

namespace JobScout.API.Controllers
{
    public class TenantController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)

    {
        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantDto dto, CancellationToken ct)
        {
            return await Handle<CreateTenantDto, CreateTenantCommand, Guid>(null, dto, ct);
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateTenant(Guid id, [FromBody] UpdateTenantDto dto, CancellationToken ct)
        {
            return await Handle<UpdateTenantDto, UpdateTenantCommand, TenantModel>(id, dto, ct);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTenant(Guid id, CancellationToken ct)
        {
            return await Handle<DeleteTenantCommand, Guid>(id, ct);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetTenantById(Guid id, CancellationToken ct)
        {
            return await Handle<GetTenantByIdQuery, TenantViewModel>(id, ct);
        }
    }
}

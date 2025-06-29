using AutoMapper;
using JobScout.App.DTO;
using JobScout.Domain.Models;
using JobScout.Core.Commands.Tenant;

namespace JobScout.App.Profiles
{
    public class TenantProfile : Profile
    {
        public TenantProfile()
        {
            CreateMap<CreateTenantDto, CreateTenantCommand>();
            CreateMap<UpdateTenantDto, UpdateTenantCommand>();
        }
    }
}

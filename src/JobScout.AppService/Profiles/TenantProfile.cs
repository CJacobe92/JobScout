using AutoMapper;
using JobScout.AppService.DTO;
using JobScout.AppService.Utilities;
using JobScout.Domain.Models;
using JobScout.Core.Commands.Tenant;

namespace JobScout.AppService.Profiles
{
    public class TenantProfile: Profile
    {
        public TenantProfile()
        {
            CreateMap<CreateTenantDto, CreateTenantCommand>();
            CreateMap<CreateTenantCommand, Tenant>()
                .ConstructUsing(cmd => new Tenant(Guid.NewGuid(), cmd.CompanyName, ShardKeyGenerator.From(cmd.CompanyName)));

        }
    }
}

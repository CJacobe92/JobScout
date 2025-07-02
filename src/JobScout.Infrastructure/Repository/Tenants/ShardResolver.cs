using System;
using JobScout.Domain.Tenants.Contracts;

namespace JobScout.Infrastructure.Repository.Tenants;

public class ShardResolver : IShardResolver
{
    public string ResolveFor(string companyName)
    {
        return "shard00";
    }
}

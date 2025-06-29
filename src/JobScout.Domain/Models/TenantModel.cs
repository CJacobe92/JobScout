using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Models;

public class TenantModel(string companyName, string shardKey)
{
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; } = companyName;
    public string ShardKey { get; private set; } = shardKey;

    public TenantModel(Guid id, string companyName, string shardKey) : this(companyName, shardKey)
    {
        Id = id;
    }

    public void Update(string? companyName, string? shardKey)
    {
        if (!string.IsNullOrWhiteSpace(companyName))
            CompanyName = companyName;

        if (!string.IsNullOrWhiteSpace(shardKey))
            ShardKey = shardKey;
    }
}
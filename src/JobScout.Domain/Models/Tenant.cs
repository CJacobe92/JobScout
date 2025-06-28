using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Domain.Models;

public class Tenant
{   
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; }
    public string ShardKey { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Tenant(string companyName, string shardKey)
    {
        CompanyName = companyName;
        ShardKey = shardKey;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public Tenant(Guid id, string companyName, string shardKey)
    {
        Id = id;
        CompanyName = companyName;
        ShardKey = shardKey;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
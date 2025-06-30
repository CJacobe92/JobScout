using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JobScout.Domain.Enumerations;

namespace JobScout.Domain.Models;

public class TenantModel(string companyName, AvailableShards shardKey)
{
    public Guid Id { get; private set; }
    public string CompanyName { get; private set; } = companyName;
    public AvailableShards ShardKey { get; private set; } = shardKey;
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public TenantModel(Guid id, string companyName, AvailableShards shardKey, DateTime createdAt, DateTime updatedAt) :
    this(companyName, shardKey)
    {
        Id = id;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void Update(string? companyName, AvailableShards? shardKey)
    {
        if (!string.IsNullOrWhiteSpace(companyName))
            CompanyName = companyName;

        if (shardKey.HasValue)
            ShardKey = shardKey.Value;
    }
}
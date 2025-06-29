using System;
using System.ComponentModel.DataAnnotations;

namespace JobScout.App.DTO;

public enum AvailableShards
{
  shard00,
  shard01,
}

public class UpdateTenantDto
{
  [Required(ErrorMessage = "Company name cannot be null or empty")]
  [StringLength(50, ErrorMessage = "Company name cannot exceed 50 characters in length")]
  public string? CompanyName { get; set; }

  [Required(ErrorMessage = "Shard is required")]
  [EnumDataType(typeof(AvailableShards), ErrorMessage = "Invalid shard key")]
  public AvailableShards? ShardKey { get; set; }
}

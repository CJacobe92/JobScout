using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using JobScout.Domain.Contracts;

namespace JobScout.Infrastructure.Database.Entities;

public class TenantEntity : IAuditableEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string CompanyName { get; set; } = default!;
    public string ShardKey { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

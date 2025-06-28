using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using JobScout.Domain.Contracts;

namespace JobScout.Infrastructure.Database.Entities;


public class Tenant
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string CompanyName { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string ShardKey { get; set; } = default!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
}
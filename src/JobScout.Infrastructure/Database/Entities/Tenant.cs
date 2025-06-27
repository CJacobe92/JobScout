using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations

namespace JobScout.Infrastructure.Database.Entities
{
    public class Tenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MinLength(1), MaxLength(50)]
        public string CompanyName { get; set; }

        [Required]
        public string ShardKey { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }

}
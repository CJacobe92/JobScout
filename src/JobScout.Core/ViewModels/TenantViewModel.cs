using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScout.Core.ViewModels;

public class TenantViewModel(Guid Id, string companyName, DateTime createdAt, DateTime updatedAt)
{
    public Guid Id { get; set; } = Id;
    public string CompanyName { get; set; } = companyName ?? throw new ArgumentNullException(nameof(companyName));

    public DateTime CreatedAt { get; set; } = createdAt;
    public DateTime UpdatedAt { get; set; } = updatedAt;

}




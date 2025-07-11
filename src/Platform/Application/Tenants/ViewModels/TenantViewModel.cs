using System;

namespace Application.Tenants.ViewModels;

public class TenantViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? License { get; set; }
    public string? Phone { get; set; }
    public string? RegisteredTo { get; set; }
    public string? TIN { get; set; }
    public string? Address { get; set; }
    public bool IsActivated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
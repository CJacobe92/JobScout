using System;
using Shared.Events.Tenants;
using Shared.SeedWork;


namespace Domain.Entities;

public class Tenant : BaseEntity, IAggregateRoot
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public string License { get; private set; }
    public string Phone { get; private set; }
    public string RegisteredTo { get; private set; }
    public string TIN { get; private set; }
    public string Address { get; private set; }
    public bool IsActivated { get; private set; } = false;
    public bool IsDeleted { get; private set; } = false;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public Tenant() { }

    private Tenant(
        string name,
        string license,
        string phone,
        string registeredTo,
        string tin,
        string address)
    {
        Name = name;
        License = license;
        Phone = phone;
        RegisteredTo = registeredTo;
        TIN = tin;
        Address = address;
    }

    public static Tenant Create(
        string name,
        string license,
        string phone,
        string registeredTo,
        string tin,
        string address)
    {

        var tenant = new Tenant(name, license, phone, registeredTo, tin, address);

        tenant.AddDomainEvent(new TenantCreatedEvent(tenant.Id, tenant.Name, tenant.License));
        return tenant;
    }

    public void Activate() => IsActivated = true;

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Update(
        string? name = null,
        string? license = null,
        string? phone = null,
        string? registeredTo = null,
        string? tin = null,
        string? address = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        if (!string.IsNullOrWhiteSpace(license)) License = license;
        if (!string.IsNullOrWhiteSpace(phone)) Phone = phone;
        if (!string.IsNullOrWhiteSpace(registeredTo)) RegisteredTo = registeredTo;
        if (!string.IsNullOrWhiteSpace(tin)) TIN = tin;
        if (!string.IsNullOrWhiteSpace(address)) Address = address;

        UpdatedAt = DateTime.UtcNow;
    }

}

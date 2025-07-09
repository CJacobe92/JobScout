using System;

namespace Domain.Entities;

public class Tenant
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
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; private set; }

    public Tenant() { }

    public Tenant(
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

    public void Activate() => IsActivated = true;

    public void MarkAsDeleted()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Update(
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
        UpdatedAt = DateTime.UtcNow;
    }
}

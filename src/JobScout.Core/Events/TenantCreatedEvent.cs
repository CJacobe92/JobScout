using JobScout.Domain.Models;
using MediatR;

namespace JobScout.Core.Events;

public record TenantCreatedEvent(TenantModel Tenant) : INotification;

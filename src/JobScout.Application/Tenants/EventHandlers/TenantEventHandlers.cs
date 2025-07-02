using System;
using System.Threading;
using System.Threading.Tasks;
using JobScout.Application.Configuration.DomainEvents;
using JobScout.Domain.Tenants;
using MediatR;

namespace JobScout.Application.Tenants.EventHandlers;

public class TenantCreatedEventHandler
    : INotificationHandler<DomainEventNotificationBase<TenantCreatedEvent>>
{
    public async Task Handle(
        DomainEventNotificationBase<TenantCreatedEvent> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // Debugging output
        Console.WriteLine($"🎯 TenantCreatedEventHandler triggered for TenantId: {domainEvent.TenantId}, CompanyName: {domainEvent.CompanyName}");

        // TODO: Add projection update, email trigger, or analytics logic here

        await Task.CompletedTask; // Simulate async work
    }
}

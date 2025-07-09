using System;

namespace Shared.Contracts.Events;

public record TenantCreatedEvent(Guid Id, string Name, string License);

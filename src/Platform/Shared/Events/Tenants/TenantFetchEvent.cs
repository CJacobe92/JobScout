using System;

namespace Shared.Events.Tenants;

public record TenantsFetchedEvent(int Count, DateTime FetchedAt);

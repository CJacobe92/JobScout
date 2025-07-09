using System;

namespace Shared.Contracts.Events;

public record TenantsFetchedEvent(int Count, DateTime FetchedAt);

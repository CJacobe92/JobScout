using System;

namespace JobScout.Domain.SeedWork;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken);

    Task DispatchFromEntityAsync(EntityBase entity, CancellationToken ct);

}

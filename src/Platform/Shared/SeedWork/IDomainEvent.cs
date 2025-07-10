using System;

namespace Shared.SeedWork;

public interface IDomainEvent
{
    DateTime OccuredOn { get; }
}

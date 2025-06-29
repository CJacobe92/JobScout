using System;

namespace JobScout.Domain.Contracts;

public interface IUnitOfWork
{
  Task BeginTransactionAsync(CancellationToken ct);
  Task CommitAsync(CancellationToken ct);
  Task RollbackAsync(CancellationToken ct);
}

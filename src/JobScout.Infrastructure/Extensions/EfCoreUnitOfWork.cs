using System;
using JobScout.Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JobScout.Infrastructure.Extensions;

public class EfCoreUnitOfWork : IUnitOfWork
{
  private readonly DbContext _context;
  private IDbContextTransaction? _transaction;

  public EfCoreUnitOfWork(DbContext context) { _context = context; }
  public async Task BeginTransactionAsync(CancellationToken ct) { if (_transaction == null) _transaction = await _context.Database.BeginTransactionAsync(ct); }
  public async Task CommitAsync(CancellationToken ct) { if (_transaction != null) { await _transaction.CommitAsync(ct); await _transaction.DisposeAsync(); _transaction = null; } }
  public async Task RollbackAsync(CancellationToken ct) { if (_transaction != null) { await _transaction.RollbackAsync(ct); await _transaction.DisposeAsync(); _transaction = null; } }
}

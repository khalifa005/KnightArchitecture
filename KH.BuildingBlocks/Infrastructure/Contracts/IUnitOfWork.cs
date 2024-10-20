using KH.BuildingBlocks.Apis.Entities;
using System.Data;

namespace KH.BuildingBlocks.Infrastructure.Contracts;

public interface IUnitOfWork : IDisposable
{
  IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
  Task<int> CommitAsync(CancellationToken cancellationToken = default);
  Task BeginTransactionAsync(CancellationToken cancellationToken = default);
  Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted, CancellationToken cancellationToken = default);
  Task RollBackTransactionAsync(CancellationToken cancellationToken = default);
  Task CommitTransactionAsync(CancellationToken cancellationToken = default);
  bool HasActiveTransaction { get; }
}

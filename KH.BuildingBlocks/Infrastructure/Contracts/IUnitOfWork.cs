using KH.BuildingBlocks.Extentions.Entities;
using System.Data;

namespace KH.BuildingBlocks.Infrastructure.Contracts;

public interface IUnitOfWork : IDisposable
{
  IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
  Task<int> CommitAsync();
  Task BeginTransactionAsync();
  Task BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted);
  Task RollBackTransactionAsync();
  Task CommitTransactionAsync();
}

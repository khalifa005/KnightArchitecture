using KH.BuildingBlocks.Extentions.Entities;

namespace KH.BuildingBlocks.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
  IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
  Task<int> CommitAsync();
  Task BeginTransactionAsync();
  Task RollBackTransactionAsync();
  Task CommitTransactionAsync();

  //Task<IReadOnlyList<T>> GetFromSql<T>(string storedName, int cmdType = 1, params Dictionary<object, object>[] parameters) where T : class;
}

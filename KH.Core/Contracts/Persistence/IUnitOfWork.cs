
using KH.Domain.Commons;
using System.Data;

namespace KH.Helper.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> Commit();
        Task BeginTransaction();
        Task RollBackTransaction();
        Task CommitTransaction();

        //Task<IReadOnlyList<T>> GetFromSql<T>(string storedName, int cmdType = 1, params Dictionary<object, object>[] parameters) where T : class;
    }
}

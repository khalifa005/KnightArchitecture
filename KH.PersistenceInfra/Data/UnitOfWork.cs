
using KH.Domain.Commons;
using KH.Helper.Contracts.Persistence;
using KH.PersistenceInfra.Repositories;

namespace KH.PersistenceInfra.Data
{
    public partial class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        //public async Task<int> BulkCommit()
        //{
        //    return _dbContext.BulkSaveChanges();
        //}

        public async Task BeginTransaction()
        {
            await _dbContext.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadUncommitted);
        }

        public async Task RollBackTransaction()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null) _repositories = new Hashtable();

            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);

                _repositories.Add(key, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[key];
        }


        //public async Task<IReadOnlyList<T>> GetFromSql<T>(string storedName, int cmdType = 1, params Dictionary<object, object>[] parameters) where T : class
        //{
        //    string parameterNames;
        //    List<SqlParameter> lstParameters = Util.GetSqlParameters(out parameterNames, cmdType, parameters);

        //    return await _dbContext.Set<T>().FromSqlRaw($"exec {storedName} {parameterNames}", lstParameters.ToArray()).ToListAsync();
        //}
    }
}

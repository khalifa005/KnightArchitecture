using KH.BuildingBlocks.Contracts.Persistence;
using KH.BuildingBlocks.Extentions.Entities;
using KH.PersistenceInfra.Data;
using KH.PersistenceInfra.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace KH.PersistenceInfra.Services
{
  /// <summary>
  /// Unit of Work implementation for managing repositories and transactions.
  /// </summary>
  public partial class UnitOfWork : IUnitOfWork
  {
    private readonly AppDbContext _dbContext;
    private ConcurrentDictionary<string, object> _repositories;
    private IDbContextTransaction _currentTransaction;

    public UnitOfWork(AppDbContext dbContext)
    {
      _dbContext = dbContext;
      _repositories = new ConcurrentDictionary<string, object>();
    }

    /// <summary>
    /// Commits the current changes to the database.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> CommitAsync()
    {
      return await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Begins a new transaction with the specified isolation level.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task BeginTransactionAsync()
    {
      if (_currentTransaction != null)
      {
        throw new InvalidOperationException("There is already an active transaction.");
      }

      _currentTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted);
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task RollBackTransactionAsync()
    {
      if (_currentTransaction != null)
      {
        await _currentTransaction.RollbackAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
      }
    }

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CommitTransactionAsync()
    {
      if (_currentTransaction == null)
      {
        throw new InvalidOperationException("There is no active transaction.");
      }

      try
      {
        //await _dbContext.SaveChangesAsync();
        await _currentTransaction.CommitAsync();
      }
      catch
      {
        await RollBackTransactionAsync();
        throw;
      }
      finally
      {
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
      }
    }

    /// <summary>
    /// Disposes the DbContext and any active transaction.
    /// </summary>
    public void Dispose()
    {
      _currentTransaction?.Dispose();
      _dbContext.Dispose();
    }

    /// <summary>
    /// Gets a repository for the specified entity type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>A repository instance for the specified entity type.</returns>
    //public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    //{
    //  var typeName = typeof(TEntity).Name;

    //  if (!_repositories.ContainsKey(typeName))
    //  {
    //    var repositoryInstance = Activator.CreateInstance(typeof(GenericRepository<>).MakeGenericType(typeof(TEntity)), _dbContext);
    //    _repositories[typeName] = repositoryInstance;
    //  }

    //  return (IGenericRepository<TEntity>)_repositories[typeName];
    //}

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
      var typeName = typeof(TEntity).Name;

      // GetOrAdd ensures thread-safe retrieval or creation of the repository instance
      return (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeName,
          (key) => Activator.CreateInstance(typeof(GenericRepository<>).MakeGenericType(typeof(TEntity)), _dbContext));
    }

  }
}

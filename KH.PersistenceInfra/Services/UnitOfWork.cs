using KH.BuildingBlocks.Apis.Entities;
using KH.BuildingBlocks.Cache.Enums;
using KH.BuildingBlocks.Cache.Interfaces;
using KH.PersistenceInfra.Data;
using KH.PersistenceInfra.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace KH.PersistenceInfra.Services;

/// <summary>
/// Unit of Work implementation for managing repositories and transactions.
/// </summary>
public partial class UnitOfWork : IUnitOfWork
{
  private readonly AppDbContext _dbContext;
  private ConcurrentDictionary<string, object> _repositories;
  private IDbContextTransaction _currentTransaction;
  private readonly ICacheService _cacheService;

  public UnitOfWork(AppDbContext dbContext, [FromKeyedServices(CacheTechEnum.Memory)] ICacheService memoryCache)
  {
    _dbContext = dbContext;
    _repositories = new ConcurrentDictionary<string, object>();
    _cacheService = memoryCache;
  }


  public bool HasActiveTransaction => _currentTransaction != null;

  /// <summary>
  /// Commits the current changes to the database.
  /// </summary>
  /// <returns>The number of state entries written to the database.</returns>
  public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.SaveChangesAsync(cancellationToken);
  }


  /// <summary>
  /// Begins a new transaction with the specified isolation level.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
  {
    if (_currentTransaction != null)
    {
      throw new InvalidOperationException("There is already an active transaction.");
    }

    _currentTransaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted, cancellationToken);
  }


  public async Task BeginTransactionAsync(System.Data.IsolationLevel isolationLevel = System.Data.IsolationLevel.ReadUncommitted, CancellationToken cancellationToken = default)
  {
    if (_currentTransaction != null)
    {
      throw new InvalidOperationException("There is already an active transaction.");
    }

    _currentTransaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);

  }


  /// <summary>
  /// Rolls back the current transaction.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  public async Task RollBackTransactionAsync(CancellationToken cancellationToken = default)
  {
    if (_currentTransaction != null)
    {
      await _currentTransaction.RollbackAsync(cancellationToken);
      await _currentTransaction.DisposeAsync();
      _currentTransaction = null;
    }
  }

  /// <summary>
  /// Commits the current transaction.
  /// </summary>
  /// <returns>A task representing the asynchronous operation.</returns>
  public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
  {
    if (_currentTransaction == null)
    {
      throw new InvalidOperationException("There is no active transaction.");
    }

    try
    {
      //await _dbContext.SaveChangesAsync();
      await _currentTransaction.CommitAsync(cancellationToken);
    }
    catch
    {
      await RollBackTransactionAsync(cancellationToken);
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

  public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
  {
    var typeName = typeof(TEntity).Name;

    // GetOrAdd ensures thread-safe retrieval or creation of the repository instance
    return (IGenericRepository<TEntity>)_repositories.GetOrAdd(typeName,
        key => Activator.CreateInstance(
            typeof(GenericRepository<>).MakeGenericType(typeof(TEntity)),
            _dbContext, _cacheService)); // Pass _cacheService along with _dbContext
  }
}

using KH.BuildingBlocks.Apis.Entities;
using KH.BuildingBlocks.Apis.Responses;
using KH.BuildingBlocks.Cache.Enums;
using KH.BuildingBlocks.Cache.Interfaces;
using KH.PersistenceInfra.Data;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace KH.PersistenceInfra.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
  private readonly AppDbContext _dbContext;
  private readonly string cacheKey = $"{typeof(T)}";
  private readonly ICacheService _cacheService;
  public GenericRepository(
    AppDbContext dbContext,
    [FromKeyedServices(CacheTechEnum.Memory)] ICacheService memoryCache)
  {
    _dbContext = dbContext;
    _cacheService = memoryCache;
  }

  public void RemoveCache()
  {
    _cacheService.Remove(cacheKey);
    //var cachedList = await _dbContext.Set<T>().ToListAsync();
    //_cacheService.Set(cacheKey, cachedList);
  }

  public IQueryable<T> GetQueryable()
  {
    return _dbContext.Set<T>();
  }

  public void Add(T entity)
  {
    _dbContext.Set<T>().Add(entity);
  }

  public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
  {
    await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
  }

  public async Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default)
  {
    await _dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
  }

  public async Task<int> CountAsync(CancellationToken cancellationToken = default)
  {
    return await _dbContext.Set<T>().CountAsync(cancellationToken);
  }
  public async Task<int> CountByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, CancellationToken cancellationToken = default)
  {
    var query = ApplyIncludes(include);
    return await query.Where(expression).CountAsync(cancellationToken);
  }

  public void Delete(T entity)
  {
    _dbContext.Entry(entity).State = EntityState.Deleted;
    _dbContext.Set<T>().Remove(entity);
  }

  public void DeleteTracked(T entity)
  {
    _dbContext.Set<T>().Remove(entity);
  }

  public async Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, CancellationToken cancellationToken = default)
  {
    var query = ApplyIncludes(include);
    return await query.Where(expression).ToListAsync(cancellationToken);
  }

  public async Task<IReadOnlyList<T>> GetAllAsync(
      Expression<Func<T, bool>> filter = null,
      Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
      bool tracking = false,
      bool useCache = true,
      CancellationToken cancellationToken = default)
  {
    var query = ApplyIncludes(include, tracking);

    if (filter != null)
    {
      query = query.Where(filter);
    }

    // Check if the type T is a LookupEntity because this will be very risky
    // if the entity is't a lookup 
    if (typeof(LookupEntity).IsAssignableFrom(typeof(T)) && useCache)
    {
      if (!_cacheService.TryGet(cacheKey, out IReadOnlyList<T> cachedList))
      {
        cachedList = await query.ToListAsync(cancellationToken);
        _cacheService.Set(cacheKey, cachedList);
      }
      return cachedList;
    }
    return await query.ToListAsync(cancellationToken);
  }

  public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, CancellationToken cancellationToken = default)
  {
    var query = ApplyIncludes(include, tracking: tracking);
    return await query.Where(expression).FirstOrDefaultAsync(cancellationToken);
  }

  public async Task<T> GetAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object?>?>? include = null, bool tracking = false, bool splitQuery = false, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
 CancellationToken cancellationToken = default)
  {
    var query = ApplyIncludes(include, tracking: tracking);

    if (orderBy != null)
    {
      query = orderBy(query);
    }

    // Enable query splitting if specified
    if (splitQuery)
    {
      query = query.AsSplitQuery();
    }

    return await query.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
  }

  public void UpdateDetachedEntity(T entity)
  {
    _dbContext.Set<T>().Attach(entity);
    _dbContext.Entry(entity).State = EntityState.Modified;
  }

  public void UpdateFromOldAndNewEntity(T entity, T newEntity)
  {
    _dbContext.Entry(entity).CurrentValues.SetValues(newEntity);
  }

  public void UpdateRange(ICollection<T> entities)
  {
    _dbContext.Set<T>().AttachRange(entities);
    foreach (var entity in entities)
    {
      _dbContext.Entry(entity).State = EntityState.Modified;
    }
  }

  public async Task<PagedList<T>> GetPagedUsingQueryAsync(int pageNumber, int pageSize, IQueryable<T> query, CancellationToken cancellationToken = default)
  {
    var count = await query.CountAsync(cancellationToken);

    var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
    return new PagedList<T>(items, count, pageNumber, pageSize);
  }

  public async Task<PagedList<TProjection>> GetPagedWithProjectionAsync<TProjection>(
      int pageNumber,
      int pageSize,
      Expression<Func<T, bool>>? filterExpression,
      Expression<Func<T, TProjection>> projectionExpression,
      Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
      Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
      bool tracking = false,
      CancellationToken cancellationToken = default)
  {
    IQueryable<T> query = _dbContext.Set<T>();

    if (include != null)
    {
      query = include(query);
    }

    if (filterExpression != null)
    {
      query = query.Where(filterExpression);
    }

    if (orderBy != null)
    {
      query = orderBy(query);
    }

    query = tracking ? query : query.AsNoTracking();

    var count = await query.CountAsync(cancellationToken);

    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(projectionExpression)
        .ToListAsync(cancellationToken);

    return new PagedList<TProjection>(items, count, pageNumber, pageSize);
  }


  private IQueryable<T> ApplyIncludes(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false)
  {
    IQueryable<T> query = _dbContext.Set<T>();

    if (include != null)
    {
      query = include(query);
    }

    return tracking ? query : query.AsNoTracking();
  }

  public async Task<int> BatchUpdateAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateExpression, CancellationToken cancellationToken = default)
  {
    var query = _dbContext.Set<T>().Where(filterExpression);
    return await query.ExecuteUpdateAsync(updateExpression, cancellationToken);
  }

  public async Task<int> BatchDeleteAsync(Expression<Func<T, bool>> filterExpression, CancellationToken cancellationToken = default)
  {
    var query = _dbContext.Set<T>().Where(filterExpression);
    return await query.ExecuteDeleteAsync(cancellationToken);
  }
  public async Task<T> ExecuteSqlSingleAsync<T>(string sql, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(sql))
    {
      throw new ArgumentException("SQL query cannot be null or empty.", nameof(sql));
    }

    if (_dbContext.Database.CurrentTransaction == null)
    {
      throw new InvalidOperationException("No active transaction found. Ensure a transaction is started before executing this query.");
    }

    var connection = _dbContext.Database.GetDbConnection();

    await using var command = connection.CreateCommand();
    command.CommandText = sql;
    command.CommandType = System.Data.CommandType.Text;

    // Use the current active transaction
    command.Transaction = _dbContext.Database.CurrentTransaction.GetDbTransaction();

    if (connection.State != System.Data.ConnectionState.Open)
    {
      await connection.OpenAsync(cancellationToken);
    }

    await using var reader = await command.ExecuteReaderAsync(cancellationToken);

    if (await reader.ReadAsync(cancellationToken))
    {
      return reader.GetFieldValue<T>(0);
    }

    return default; // Handle case where no rows are returned
  }

}

using KH.BuildingBlocks.Apis.Entities;
using KH.BuildingBlocks.Apis.Responses;
using KH.PersistenceInfra.Data;
using Microsoft.EntityFrameworkCore.Query;

namespace KH.PersistenceInfra.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
  private readonly AppDbContext _dbContext;

  public GenericRepository(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public IQueryable<T> GetQueryable()
  {
    return _dbContext.Set<T>();
  }

  public void Add(T entity)
  {
    _dbContext.Set<T>().Add(entity);
  }

  public async Task AddAsync(T entity)
  {
    await _dbContext.Set<T>().AddAsync(entity);
  }

  public async Task AddRangeAsync(ICollection<T> entities)
  {
    await _dbContext.Set<T>().AddRangeAsync(entities);
  }

  public int Count()
  {
    return _dbContext.Set<T>().Count();
  }

  public async Task<int> CountAsync()
  {
    return await _dbContext.Set<T>().CountAsync();
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

  public IReadOnlyList<T> FindBy(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    var query = ApplyIncludes(include);
    return query.Where(expression).ToList();
  }

  public async Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    var query = ApplyIncludes(include);
    return await query.Where(expression).ToListAsync();
  }

  public async Task<int> CountByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    var query = ApplyIncludes(include);
    return await query.Where(expression).CountAsync();
  }

  public IReadOnlyList<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    var query = ApplyIncludes(include);
    return query.ToList();
  }

  public async Task<IReadOnlyList<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    var query = ApplyIncludes(include);
    return await query.ToListAsync();
  }

  public async Task<List<T>> GetAllWithTrackingAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    var query = ApplyIncludes(include, tracking: true);
    return await query.ToListAsync();
  }

  public T Get(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    var query = ApplyIncludes(include);
    return query.FirstOrDefault(t => t.Id == id);
  }

  public async Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false)
  {
    var query = ApplyIncludes(include, tracking: tracking);
    return await query.Where(expression).FirstOrDefaultAsync();
  }

  public async Task<T> GetAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object?>?>? include = null, bool tracking = false, bool splitQuery = false)
  {
    var query = ApplyIncludes(include, tracking: tracking);

    // Enable query splitting if specified
    if (splitQuery)
    {
      query = query.AsSplitQuery();
    }

    return await query.FirstOrDefaultAsync(t => t.Id == id);
  }
  public void UpdateDetachedEntity(T entity)
  {
    //Marks the entire entity as modified and will result in all columns being updated.
    _dbContext.Set<T>().Attach(entity);
    _dbContext.Entry(entity).State = EntityState.Modified;

  }
  public void UpdateFromOldAndNewEntity(T entity, T newEntity)
  {
    //CurrentValues.SetValues, EF Core will update only the properties that differ from the tracked entity in the context.
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


  public async Task<PagedList<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
  {
    IQueryable<T> query = _dbContext.Set<T>();

    if (include != null)
    {
      query = include(query);
    }

    query = query.Where(expression);

    var count = await query.CountAsync();

    var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    return new PagedList<T>(items, count, pageNumber, pageSize);

    //return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
  }

  public async Task<PagedList<T>> GetPagedUsingQueryAsync(int pageNumber, int pageSize, IQueryable<T> query)
  {

    var count = await query.CountAsync();

    var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    return new PagedList<T>(items, count, pageNumber, pageSize);

    //return await PagedList<T>.CreateAsync(query, pageNumber, pageSize);
  }

  public async Task<PagedList<TProjection>> GetPagedWithProjectionAsync<TProjection>(
  int pageNumber,
  int pageSize,
  Expression<Func<T, bool>> filterExpression, // Filter expression
  Expression<Func<T, TProjection>> projectionExpression, // Projection (Select)
  Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, // Includes
  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // Sorting
  bool tracking = false // Whether to track entities
)
  {
    // Start with the base entity set
    IQueryable<T> query = _dbContext.Set<T>();

    // Apply includes if specified
    if (include != null)
    {
      query = include(query);
    }

    // Apply the filter
    if (filterExpression != null)
    {
      query = query.Where(filterExpression);
    }

    // Apply ordering if specified
    if (orderBy != null)
    {
      query = orderBy(query);
    }

    // Apply tracking or no-tracking based on the parameter
    query = tracking ? query : query.AsNoTracking();

    // Get the total count of items that match the filter
    var count = await query.CountAsync();

    // Apply pagination and projection
    var items = await query
        .Skip((pageNumber - 1) * pageSize)
        .Take(pageSize)
        .Select(projectionExpression) // Project only the required columns
        .ToListAsync();

    // Return paged list with projected results
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


  public async Task<int> BatchUpdateAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateExpression)
  {
    var query = _dbContext.Set<T>().Where(filterExpression);

    // Use ExecuteUpdateAsync to update entities in bulk
    return await query.ExecuteUpdateAsync(updateExpression);
  }

  public async Task<int> BatchDeleteAsync(Expression<Func<T, bool>> filterExpression)
  {
    var query = _dbContext.Set<T>().Where(filterExpression);

    // Use ExecuteDeleteAsync to delete entities in bulk
    return await query.ExecuteDeleteAsync();
  }


}

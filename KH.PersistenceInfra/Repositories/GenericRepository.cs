using KH.Domain.Commons;
using KH.Helper.Contracts.Persistence;
using KH.Helper.Responses;
using KH.PersistenceInfra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace KH.PersistenceInfra.Repositories
{
  /// <summary>
  /// Generic repository implementation for managing entities.
  /// </summary>
  /// <typeparam name="T">The type of the entity.</typeparam>
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
      var query = ApplyIncludes(include, tracking:true);
      return await query.ToListAsync();
    }

    public T Get(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
      var query = ApplyIncludes(include);
      return query.FirstOrDefault(t => t.Id == id);
    }

    public async Task<T> GetAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
      var query = ApplyIncludes(include);
      return await query.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<T> GetAsyncTracking(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
    {
      var query = ApplyIncludes(include, tracking: true);
      return await query.FirstOrDefaultAsync(t => t.Id == id);
    }

    public void Update(T entity)
    {
      _dbContext.Set<T>().Attach(entity);
      _dbContext.Entry(entity).State = EntityState.Modified;
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

    private IQueryable<T> ApplyIncludes(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false)
    {
      IQueryable<T> query = _dbContext.Set<T>();

      if (include != null)
      {
        query = include(query);
      }

      return tracking ? query : query.AsNoTracking();
    }
  }
}

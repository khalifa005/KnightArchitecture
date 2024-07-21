using KH.Domain.Commons;
using KH.Helper.Contracts.Persistence;
using KH.PersistenceInfra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Security.Policy;

namespace KH.PersistenceInfra.Repositories
{
  public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
  {
    private readonly AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
      _dbContext = dbContext;
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
      //??
      _dbContext.Entry(entity).State = EntityState.Deleted;

      _dbContext.Set<T>().Remove(entity);
    }

    public IReadOnlyList<T> FindBy(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] expressions)
    {
      return ApplyIncludes(includes: expressions).Where(expression).ToList();
    }

    public async Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] expressions)
    {
      return await ApplyIncludes(includes: expressions).Where(expression).AsNoTracking().ToListAsync();
    }

    public async Task<int> CountByAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] expressions)
    {
      return await ApplyIncludes(includes: expressions).Where(expression).CountAsync();
    }

    public async Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> customInclude = null)
    {
      return await ApplyIncludeAsyncs(includes: customInclude).Where(expression).AsNoTracking().ToListAsync();
      //return await ApplyIncludes(includes: expressions).Where(expression).AsNoTracking().ToListAsync();
    }



    public async Task<IReadOnlyList<T>> FindByIncAsync(Expression<Func<T, bool>> expression, string[] includeStrings = null, params Expression<Func<T, object>>[] expressions)
    {
      return await ApplyIncludes(includeStrings, includes: expressions).Where(expression).ToListAsync();
    }


    public IReadOnlyList<T> GetAll(params Expression<Func<T, object>>[] expressions)
    {
      return ApplyIncludes(includes: expressions).ToList();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] expressions)
    {
      return await ApplyIncludes(includes: expressions).ToListAsync();
    }

    public T Get(int id, params Expression<Func<T, object>>[] expressions)
    {
      return ApplyIncludes(includes: expressions).FirstOrDefault(t => t.Id == id);
    }

    public async Task<T> GetAsync(int id, params Expression<Func<T, object>>[] expressions)
    {
      return await ApplyIncludes(includes: expressions).FirstOrDefaultAsync(t => t.Id == id);
    }
    public async Task<T> GetAsyncTracking(int id, params Expression<Func<T, object>>[] expressions)
    {
      return await ApplyIncludesTracking(includes: expressions).FirstOrDefaultAsync(t => t.Id == id);
    }

    public void Update(T entity)
    {
      _dbContext.Set<T>().Attach(entity);
      _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public void UpdateRange(ICollection<T> entities)
    {
      _dbContext.Set<T>().AttachRange(entities);
      _dbContext.Entry(entities).State = EntityState.Modified;
    }

    private IQueryable<T> ApplyIncludes(string[] includeStrings = null, params Expression<Func<T, object>>[] includes)
    {

      //query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));


      IQueryable<T> query = _dbContext.Set<T>();

      if (includes != null)
      {
        query = includes.Aggregate(query, (current, include) => current.Include(include));
      }
      //foreach (var include in includes)
      //{
      //    query = query.Include(include);
      //}

      //if (includeStrings != null)
      //{
      //    foreach (var include in includeStrings)
      //    {
      //        query = query.Include(include);
      //    }
      //}

      return query.AsNoTracking();
    }

    private IQueryable<T> ApplyIncludeAsyncs(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
    {
      IQueryable<T> query = _dbContext.Set<T>();

      if (includes != null)
      {
        query = includes(query);
      }

      return query.AsNoTracking();
    }
    private IQueryable<T> ApplyIncludesTracking(string[] includeStrings = null, params Expression<Func<T, object>>[] includes)
    {
      IQueryable<T> query = _dbContext.Set<T>();

      foreach (var include in includes)
      {
        query = query.Include(include);
      }

      if (includeStrings != null)
      {
        foreach (var include in includeStrings)
        {
          query = query.Include(include);
        }
      }

      return query;
    }

    public IQueryable<T> GetQueryable()
    {
      return _dbContext.Set<T>();
    }
  }
}

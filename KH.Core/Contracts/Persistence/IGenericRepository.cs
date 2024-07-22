
using KH.Domain.Commons;
using Microsoft.EntityFrameworkCore.Query;

namespace KH.Helper.Contracts.Persistence
{
  public interface IGenericRepository<T> where T : BaseEntity
  {
    IQueryable<T> GetQueryable();
    void Add(T entity);
    Task AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);
    int Count();
    Task<int> CountAsync();
    void Delete(T entity);
    IReadOnlyList<T> FindBy(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<int> CountByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    IReadOnlyList<T> GetAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<IReadOnlyList<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    T Get(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<T> GetAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    Task<T> GetAsyncTracking(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
    void Update(T entity);
    void UpdateRange(ICollection<T> entities);
  }
}

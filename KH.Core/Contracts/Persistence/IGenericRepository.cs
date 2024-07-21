
using KH.Domain.Commons;
using Microsoft.EntityFrameworkCore.Query;

namespace KH.Helper.Contracts.Persistence
{
  public interface IGenericRepository<T> where T : BaseEntity
  {
    T Get(int id, params Expression<Func<T, object>>[] includeExpressions);
    Task<T> GetAsync(int id, params Expression<Func<T, object>>[] includeExpressions);
    Task<T> GetAsyncTracking(int id, params Expression<Func<T, object>>[] expressions);
    IReadOnlyList<T> GetAll(params Expression<Func<T, object>>[] includeExpressions);
    Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includeExpressions);
    IReadOnlyList<T> FindBy(Expression<Func<T, bool>> whereExpression, params Expression<Func<T, object>>[] includeExpressions);

    Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> customInclude = null);
    Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> whereExpression, params Expression<Func<T, object>>[] includeExpressions);
    Task<IReadOnlyList<T>> FindByIncAsync(Expression<Func<T, bool>> whereExpression, string[] includeStrings = null, params Expression<Func<T, object>>[] includeExpressions);
    IQueryable<T> GetQueryable();
    int Count();
    Task<int> CountByAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] expressions);
    Task<int> CountAsync();

    void Add(T entity);
    Task AddAsync(T entity);
    Task AddRangeAsync(ICollection<T> entities);
    void Update(T entity);
    void UpdateRange(ICollection<T> entities);
    void Delete(T entity);
    //Task<IReadOnlyList<T>> GetFromSql(string storedName, int cmdType = 1, params Dictionary<object, object>[] parameters);
    //Task<int> OperateFromSql(string storedName, int cmdType = 2, params Dictionary<object, object>[] parameters);
  }
}

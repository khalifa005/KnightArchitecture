using KH.BuildingBlocks.Extentions.Entities;
using KH.BuildingBlocks.Responses;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace KH.BuildingBlocks.Contracts.Persistence;

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
  Task<List<T>> GetAllWithTrackingAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
  Task<IReadOnlyList<T>> GetAllAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
  T Get(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
  //Task<T> GetAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
  Task<T> GetAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false);
  //Task<T> GetAsyncTracking(long id, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
  Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
  void Update(T entity);
  void UpdateX(T entity, T newEntity);
  void UpdateRange(ICollection<T> entities);
  Task<PagedList<T>> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
  Task<PagedList<T>> GetPagedUsingQueryAsync(int pageNumber, int pageSize, IQueryable<T> query);
  Task<PagedList<TProjection>> GetPagedWithProjectionAsync<TProjection>(
  int pageNumber,
  int pageSize,
  Expression<Func<T, bool>> filterExpression, // Filter expression
  Expression<Func<T, TProjection>> projectionExpression, // Projection (Select)
  Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, // Includes
  Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, // Sorting
  bool tracking = false);

}

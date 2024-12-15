using KH.BuildingBlocks.Apis.Entities;
using Microsoft.EntityFrameworkCore.Query;
using StackExchange.Redis;
using System.Linq.Expressions;

namespace KH.BuildingBlocks.Infrastructure.Contracts;
public interface IGenericRepository<T> where T : BaseEntity
{
  IQueryable<T> GetQueryable();
  void Add(T entity);
  Task AddAsync(T entity, CancellationToken cancellationToken = default);
  Task AddRangeAsync(ICollection<T> entities, CancellationToken cancellationToken = default);
  Task<int> CountAsync(CancellationToken cancellationToken = default);
  void Delete(T entity);
  void DeleteTracked(T entity);
  Task<IReadOnlyList<T>> FindByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, CancellationToken cancellationToken = default);
  Task<int> CountByAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, CancellationToken cancellationToken = default);

   Task<IReadOnlyList<T>> GetAllAsync(
    Expression<Func<T, bool>> filter = null,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
    bool tracking = false,
    bool useCache = true,
    CancellationToken cancellationToken = default);
  Task<T> GetAsync(long id, Func<IQueryable<T>, IIncludableQueryable<T, object?>?>? include = null, bool tracking = false, bool splitQuery = false, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, CancellationToken cancellationToken = default);
  Task<T> GetByExpressionAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool tracking = false, CancellationToken cancellationToken = default);
  void UpdateDetachedEntity(T entity);
  void UpdateFromOldAndNewEntity(T entity, T newEntity);
  void UpdateRange(ICollection<T> entities);
  Task<PagedList<T>> GetPagedUsingQueryAsync(int pageNumber, int pageSize, IQueryable<T> query, CancellationToken cancellationToken = default);
  Task<PagedList<TProjection>> GetPagedWithProjectionAsync<TProjection>(
       int pageNumber,
       int pageSize,
       Expression<Func<T, bool>>? filterExpression,
       Expression<Func<T, TProjection>> projectionExpression,
       Func<IQueryable<T>, IIncludableQueryable<T, object?>>? include = null,
       Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
       bool tracking = false,
       CancellationToken cancellationToken = default);
  Task<int> BatchUpdateAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateExpression, CancellationToken cancellationToken = default);
  Task<int> BatchDeleteAsync(Expression<Func<T, bool>> filterExpression, CancellationToken cancellationToken = default);
  void RemoveCache();
  Task<T> ExecuteSqlRawAsync(string sql, CancellationToken cancellationToken = default);

}

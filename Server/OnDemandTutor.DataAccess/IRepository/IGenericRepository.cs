using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Paging;

namespace OnDemandTutor.DataAccess.IRepository;

public interface IGenericRepository<TEntity> where TEntity : class, IBaseEntity
{
    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(CancellationToken cancellationToken = default);

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default);

    Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default);

    TEntity Find(params object[] keyValues);

    ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken);

    ValueTask<TEntity> FindAsync(params object[] keyValues);

    TEntity FirstOrDefault();

    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

    Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default);

    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default);

    EntityEntry<TEntity> Add(TEntity entity);

    ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    void AddRange(IEnumerable<TEntity> entities);

    void AddRange(params TEntity[] entities);

    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task AddRangeAsync(params TEntity[] entities);

    EntityEntry<TEntity> Attach(TEntity entity);

    void AttachRange(params TEntity[] entities);

    void AttachRange(IEnumerable<TEntity> entities);

    EntityEntry<TEntity> Update(TEntity entity);

    void UpdateRange(IEnumerable<TEntity> entities);

    void UpdateRange(params TEntity[] entities);

    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);

    EntityEntry<TEntity> Remove(TEntity entity);

    void RemoveRange(IEnumerable<TEntity> entities);

    void RemoveRange(params TEntity[] entities);

    Task<PagedResult<TEntity>> PagingAsync<T>(PagingModel<T> request);

    Task<PagedResult<TEntity>> PagingAsync<T>(PagingModel<T> request, Expression<Func<TEntity, bool>> predicate);

    Task<PagedResult<T>> PagingAsync<F, T>(PagingModel<F> request, Func<List<TEntity>, List<T>> mapping);

    Task<PagedResult<T>> ToPagingAsync<T, TEntity>(
        IQueryable<TEntity> query,
        int page, int limit);

    Task<PagedResult<T>> PagingAsync<F, T>(PagingModel<F> request,
        Expression<Func<TEntity, bool>> predicate, Func<List<TEntity>, List<T>> mapping);

    Task<PagedResult<T>> PagingAsync<F, T>(PagingModel<F> request,
        Expression<Func<TEntity, bool>> predicate, Func<List<TEntity>, List<T>> mapping,
        params (string key, string val)[] propertyMapping);

    Task<List<T>> GetOptionModelAsync<T>(
        Expression<Func<TEntity, bool>> predicate,
        Expression<Func<TEntity, T>> selector
    );
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OnDemandTutor.DataAccess.Helper;
using OnDemandTutor.DataAccess.IRepository;
using OnDemandTutor.Models;
using OnDemandTutor.Models.Paging;
using System.Linq.Expressions;

namespace OnDemandTutor.DataAccess.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly ApplicationDbContext context;

        protected readonly DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }

        public virtual Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.AllAsync(predicate, cancellationToken);

        public virtual Task<bool> AnyAsync(CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.AnyAsync();

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.AnyAsync(predicate, cancellationToken);

        public virtual Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.CountAsync(predicate, cancellationToken);

        public virtual Task<TResult> MaxAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.MaxAsync(selector, cancellationToken);

        public virtual Task<TResult> MinAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.MinAsync(selector, cancellationToken);

        public virtual TEntity Find(params object[] keyValues)
            => dbSet.Find(keyValues);

        public virtual ValueTask<TEntity> FindAsync(object[] keyValues, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.FindAsync(keyValues, cancellationToken);

        public virtual ValueTask<TEntity> FindAsync(params object[] keyValues)
            => dbSet.FindAsync(keyValues);

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dbSet.Where(predicate);
        }

        public virtual Task<List<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate)
            => dbSet.Where(predicate).ToListAsync();

        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
            => dbSet.FirstOrDefault(predicate);

        public virtual TEntity FirstOrDefault()
            => dbSet.FirstOrDefault();

        public virtual Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        public virtual Task<TEntity> FirstOrDefaultAsync(CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.FirstOrDefaultAsync(cancellationToken);

        public virtual Task<List<TEntity>> ToListAsync(CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.ToListAsync(cancellationToken);

        public virtual EntityEntry<TEntity> Add(TEntity entity)
            => dbSet.Add(entity);

        public virtual ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.AddAsync(entity, cancellationToken);

        public virtual void AddRange(IEnumerable<TEntity> entities)
            => dbSet.AddRange(entities);

        public virtual void AddRange(params TEntity[] entities)
            => dbSet.AddRange(entities);

        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default(CancellationToken))
            => dbSet.AddRangeAsync(entities, cancellationToken);

        public virtual Task AddRangeAsync(params TEntity[] entities)
            => dbSet.AddRangeAsync(entities);

        public virtual EntityEntry<TEntity> Attach(TEntity entity)
            => dbSet.Attach(entity);

        public virtual void AttachRange(params TEntity[] entities)
            => dbSet.AttachRange(entities);

        public virtual void AttachRange(IEnumerable<TEntity> entities)
            => dbSet.AttachRange(entities);

        public virtual EntityEntry<TEntity> Update(TEntity entity)
            => dbSet.Update(entity);

        public virtual void UpdateRange(params TEntity[] entities)
            => dbSet.UpdateRange(entities);

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
            => dbSet.UpdateRange(entities);

        public virtual EntityEntry<TEntity> Remove(TEntity entity)
            => dbSet.Remove(entity);
        public virtual void RemoveRange(params TEntity[] entities)
            => RemoveRange(entities.ToList());

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
            => entities.ToList().ForEach(entity => Remove(entity));


        public virtual Task<PagedResult<TEntity>> PagingAsync<T>(PagingModel<T> request)
        {
            return this.dbSet
               .OrderProperty(request.Sorts)
               .ToPagingAsync<TEntity, TEntity>(request.Page, request.Limit);
        }

        public virtual Task<PagedResult<T>> PagingAsync<F, T>(PagingModel<F> request, Func<List<TEntity>, List<T>> mapping)
        {
            return this.dbSet
               .OrderProperty(request.Sorts)
               .ToPagingAsync(request.Page, request.Limit, mapping);
        }

        public virtual Task<PagedResult<TEntity>> PagingAsync<T>(PagingModel<T> request, Expression<Func<TEntity, bool>> predicate) =>
            this.dbSet
               .Where(predicate)
               .OrderProperty(request.Sorts)
               .ToPagingAsync<TEntity, TEntity>(request.Page, request.Limit);


        public virtual Task<PagedResult<T>> PagingAsync<F, T>(PagingModel<F> request,
            Expression<Func<TEntity, bool>> predicate, Func<List<TEntity>, List<T>> mapping)
       => this.dbSet
                .Where(predicate)
                .OrderProperty(request.Sorts)
                .ToPagingAsync(request.Page, request.Limit, mapping);


        public Task<PagedResult<T>> PagingAsync<F, T>(PagingModel<F> request,
            Expression<Func<TEntity, bool>> predicate,
            Func<List<TEntity>, List<T>> mapping,
            params (string key, string val)[] propertyMapping)
         => this.dbSet
                .Where(predicate)
                .OrderProperty(request.Sorts, propertyMapping)
                .ToPagingAsync(request.Page, request.Limit, mapping);


        public Task<List<T>> GetOptionModelAsync<T>(
             Expression<Func<TEntity, bool>> predicate,
             Expression<Func<TEntity, T>> selector
             )
        {
            return dbSet.Where(predicate)
                   .Select(selector)
                   .ToListAsync();
        }

    }
}

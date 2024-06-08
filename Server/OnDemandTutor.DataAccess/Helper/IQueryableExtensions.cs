using Microsoft.EntityFrameworkCore;
using OnDemandTutor.Helper;
using OnDemandTutor.Models.Paging;
using System.Linq.Expressions;
using System.Reflection;

namespace OnDemandTutor.DataAccess.Helper
{
    public static class IQueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagingAsync<T, TEntity>(
          this IQueryable<TEntity> query,
          int page, int limit,
          Func<List<TEntity>, List<T>>? mapper = null
          )
        {
            var result = new PagedResult<T>
            {
                Total = await query.CountAsync(),
                Page = page,
                Limit = limit
            };

            List<TEntity> items;
            if (limit > 0)
            {
                var startIndex = page * limit;
                items = await query
                    .Skip(startIndex < 0 ? 0 : startIndex)
                    .Take(limit)
                    .ToListAsync();
            }
            else
            {
                items = await query.ToListAsync();
            }
            if (mapper != null)
            {
                result.Items = mapper.Invoke(items ?? new List<TEntity>());
            }
            else
            {
                result.Items = (List<T>)(object)(items ?? new List<TEntity>());
            }

            return result;
        }

        public static IQueryable<T> OrderProperty<T>(
            this IQueryable<T> query,
            List<SortItems> sortItems,
            params (string key, string val)[] propertyMapping)
        {
            IOrderedQueryable<T>? result = null;
            if (sortItems == null || !sortItems.Any()) return query;

            for (int i = 0; i < sortItems.Count; i++)
            {
                if (i == 0)
                {
                    if (sortItems[i].IsDesc)
                        result = query.OrderByDescending(getProperty(sortItems[i].Column));
                    else
                        result = query.OrderBy(getProperty(sortItems[i].Column));
                }
                else
                {
                    if (sortItems[i].IsDesc)
                        result = result?.ThenByDescending(getProperty(sortItems[i].Column));
                    else
                        result = result?.ThenBy(getProperty(sortItems[i].Column));
                }
            }

            return result ?? query;


            string getProperty(string property)
            {
                var key = property.ToPascalCase();
                var map = propertyMapping?.FirstOrDefault(x => x.key == key);
                return map.HasValue && map.Value.val != null ? map.Value.val : key;
            }
        }

        public static IOrderedQueryable<T> OrderBy<T>(
            this IQueryable<T> source,
            string property)
        {
            return ApplyOrder(source, property, "OrderBy");
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(
            this IQueryable<T> source,
            string property)
        {
            return ApplyOrder(source, property, "OrderByDescending");
        }

        public static IOrderedQueryable<T> ThenBy<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            return ApplyOrder(source, property, "ThenBy");
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(
            this IOrderedQueryable<T> source,
            string property)
        {
            return ApplyOrder(source, property, "ThenByDescending");
        }

        static IOrderedQueryable<T> ApplyOrder<T>(
            IQueryable<T> source,
            string property,
            string methodName)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ
                PropertyInfo pi = type.GetProperty(prop);
                if (pi is null) continue;
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, expr, arg);

            object result = typeof(Queryable).GetMethods().Single(
                    method => method.Name == methodName
                            && method.IsGenericMethodDefinition
                            && method.GetGenericArguments().Length == 2
                            && method.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(T), type)
                    .Invoke(null, new object[] { source, lambda });
            return (IOrderedQueryable<T>)result;
        }

    }
}

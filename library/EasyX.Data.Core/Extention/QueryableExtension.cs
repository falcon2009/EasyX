using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using EasyX.Data.Api.Enum;
using EasyX.Data.Api.Filter;
using EasyX.Data.Api.Request;
using EasyX.Data.Core.Internal;
using EasyX.Infra;

namespace EasyX.Data.Core.Extention
{
    public static class QueryableExtension
    {
        public static IQueryable<TEntity> ApplyFilter<TEntity>(this IQueryable<TEntity> query, IEnumerable<IFilterItem> filterList) where TEntity : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), Constant.Errors.QueryNull);
            }
            if (filterList == null)
            {
                return query;
            }

            foreach (IFilterItem filter in filterList)
            {
                Expression<Func<TEntity, bool>> otherPredicate = PredicateProvider.GetPredicateForFilter<TEntity>(filter);
                if (otherPredicate != null)
                {
                    query = query.Where(otherPredicate);
                }
            }

            return query;
        }
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query, int? skip, int? take) where TEntity : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), Constant.Errors.QueryNull);
            }

            if (skip.HasValue)
            {
                query = query.Skip(skip.Value);
            }
            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }
            return query;
        }
        public static IQueryable<TEntity> ApplySorting<TEntity>(this IQueryable<TEntity> query, ISortItem sort) where TEntity : class
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), Constant.Errors.QueryNull);
            }
            if (string.IsNullOrEmpty(sort?.Field))
            {
                return query;
            }

            //Expression<Func<TEntity, object>> predicate = GetPredicateForSorting<TEntity>(sort.Field);
            Expression<Func<TEntity, object>> predicate = PredicateProvider.GetPredicateForSorting<TEntity>(sort.Field);
            IOrderedQueryable<TEntity> orderedQuery = (sort.Order == SortOrder.Descending) ? Queryable.OrderByDescending(query, predicate) 
                                                                                           : Queryable.OrderBy(query, predicate);

            return orderedQuery;
        }
        public static Expression<Func<TEntity, bool>> GetPredicateForEntity<TEntity, TKey>(TKey key) where TEntity : class where TKey : class
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key), Constant.Errors.KeyNull);
            }

            return PredicateProvider.GetPredicateForEntity<TEntity, TKey>(key);
        }

    }
}

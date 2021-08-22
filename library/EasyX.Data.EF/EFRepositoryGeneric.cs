using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using EasyX.Data.Api.Entity;
using EasyX.Data.Api.Enum;
using EasyX.Data.Api.Filter;
using EasyX.Data.Api.Request;
using EasyX.Data.Core.Extention;
using EasyX.Data.Core.Request;
using EasyX.Data.Core.Internal;
using EasyX.Infra;

namespace EasyX.Data.EF
{
    public abstract class EFRepositoryGeneric<TContext, TEntity, TKey> : IDisposable, IEFRepository<TEntity, TKey> where TContext : DbContext where TEntity : class, IKey<TKey> where TKey : class
    {
        private readonly List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> customerFilterList = new();
        private readonly Dictionary<string, IQueryable<TEntity>> queryStore = new();
        private readonly Dictionary<string, Func<IQueryable<TEntity>, object>> selectStore = new();
        private bool disposed;

        protected EFRepositoryGeneric(TContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
            Initialize();
        }

        public virtual async Task<TModel> GetModelAsync<TModel>(IKey<TKey> key, bool isTrackable = default, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>
        {
            (IQueryable<TModel> modelQuery, Expression<Func<TModel, bool>> predicate) = GetQueryAndPredicateForModel<TModel>(key, isTrackable);

            return await modelQuery.FirstOrDefaultAsync(predicate, cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task<TModel> GetFirstFromModelAsync<TModel>(IRequest request, CancellationToken cancellationToken = default) where TModel : class
        {
            return await GetQueryForModelList<TModel>(request).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual async Task<List<TModel>> GetModelListAsync<TModel>(IRequest request, CancellationToken cancellationToken = default) where TModel : class
        {
            List<TModel> modelList = await GetQueryForModelList<TModel>(request).ToListAsync(cancellationToken).ConfigureAwait(false);

            return modelList;
        }
        public virtual async Task<int> GetTotalAsync<TModel>(IRequest request, CancellationToken cancellationToken = default) where TModel : class
        {
            return await GetQueryForTotal<TModel>(request).CountAsync(cancellationToken).ConfigureAwait(false);
        }
        public virtual Task<int> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Add(entity);
            Task<int> task = Context.SaveChangesAsync(cancellationToken);

            return task;
        }
        public virtual Task<int> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            Task<int> task = Context.SaveChangesAsync(cancellationToken);

            return task;
        }
        public virtual Task<int> DeleteAsync(IKey<TKey> key, CancellationToken cancellationToken = default)
        {
            SetEntityToDelete(key);
            Task<int> task = Context.SaveChangesAsync(cancellationToken);

            return task;
        }
        public DbContext GetContext() => Context;
        public void AddFilter(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFilter)
        {
            customerFilterList.Add(queryFilter);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region protected
        protected DbSet<TEntity> DbSet { get; private set; }
        protected TContext Context { get; private set; }
        protected virtual void Dispose(bool isDispose)
        {
            if (disposed)
            {
                return;
            }
            Context.Dispose();
            disposed = true;
        }
        protected abstract void ConfigureRepository();
        protected abstract string DefaultSortField { get; }
        protected virtual void FixFilter(IRequest filter)
        { }
        protected void AddQueryForModel<TEntityModel>(Func<IQueryable<TEntity>, IQueryable<TEntityModel>> select, IQueryable<TEntity> query)
        {
            string key = typeof(TEntityModel).Name;
            AddQuery(key, query);
            AddSelect(key, select);
        }
        protected void AddQuery(string key, IQueryable<TEntity> query)
        {
            if (!queryStore.ContainsKey(key))
            {
                queryStore.Add(key, query);
            }
        }
        protected void AddSelect<TEntityModel>(string key, Func<IQueryable<TEntity>, IQueryable<TEntityModel>> select)
        {
            if (!selectStore.ContainsKey(key) && select != null)
            {
                selectStore.Add(key, select);
            }
        }
        #endregion

        #region private
        private void Initialize()
        {
            ConfigureRepository();
        }
        private (IQueryable<TModel> query, Expression<Func<TModel, bool>> predicate) GetQueryAndPredicateForModel<TModel>(IKey<TKey> keyProvider, bool isTrackable = default) where TModel : class, IKey<TKey>
        {
            if (keyProvider == null)
            {
                throw new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull);
            }

            string modelName = typeof(TModel).Name;
            //get the key
            TKey primaryKey = keyProvider.Key;
            //get query and select function for a certain model
            (IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TModel>> select) tool = GetQueryAndSelect<TModel>();
            IQueryable<TEntity> query = tool.query;
            Func<IQueryable<TEntity>, IQueryable<TModel>> select = tool.select;
            if (query == null)
            {
                string message = $"Query for {modelName} is undefined";
                throw new ArgumentException(message);
            }

            //turm off tracking if required
            if (!isTrackable)
            {
                query = query.AsNoTracking();
            }

            //check if TEntity and TModel is the same. apply select function to get a certain model;
            IQueryable<TModel> modelQuery;
            if (typeof(TModel) == typeof(TEntity))
            {
                modelQuery = query as IQueryable<TModel>;
            }
            else
            {
                if (select == null)
                {
                    string message = $"Select expresion for {modelName} is undefined";
                    throw new ArgumentNullException(typeof(TModel).Name, message);
                }

                modelQuery = select(query);
            }


            //get predicate
            Expression<Func<TModel, bool>> predicate = PredicateProvider.GetPredicateForEntity<TModel, TKey>(primaryKey);

            return (modelQuery, predicate);
        }
        private IQueryable<TModel> GetQueryForModelList<TModel>(IRequest filter) where TModel : class
        {
            string modelName = typeof(TModel).Name;
            //get query and select function for a certain model
            (IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TModel>> select) tool = GetQueryAndSelect<TModel>();
            IQueryable<TEntity> query = tool.query;
            Func<IQueryable<TEntity>, IQueryable<TModel>> select = tool.select;
            if (query == null)
            {
                string message = $"Query for {modelName} is undefined";
                throw new ArgumentException(message);
            }
            //turn of tracking
            query = query.AsNoTracking();
            //apply sorting and paging
            ISortItem sortParam = filter?.SortItem ?? new SortItem(DefaultSortField, SortOrder.Descending);
            int? skip = filter?.Skip;
            int? take = filter?.Take;
            query = query.ApplySorting(sortParam)
                         .ApplyPaging(skip, take);

            //apply customer changes for filter
            FixFilter(filter);
            //get filter params
            IEnumerable<IFilterItem> filterItemList = filter?.GetFilterItemList();
            bool hasFilter = filterItemList != null;
            //apply filters from request
            if (hasFilter)
            {
                query = query.ApplyFilter(filterItemList);
            }
            //apply customer filters
            customerFilterList.ForEach(customFilter => query = customFilter(query));

            //apply select function to get a certain model;
            if (typeof(TModel) == typeof(TEntity))
            {
                return query as IQueryable<TModel>;
            }
            if (select == null)
            {
                string message = $"Select expresion for {modelName} is undefined";
                throw new ArgumentNullException(typeof(TModel).Name, message);
            }
            IQueryable<TModel> modelQuery = select(query);

            string queryString = modelQuery.ToString();
            Console.WriteLine(queryString);

            return modelQuery;
        }
        private IQueryable<TEntity> GetQueryForTotal<TModel>(IRequest request) where TModel : class
        {

            string modelName = typeof(TModel).Name;
            IQueryable<TEntity> query = queryStore.ContainsKey(modelName) ? queryStore[modelName] : null;
            if (query == null)
            {
                string message = $"Query for {modelName} is undefined";
                throw new ArgumentException(message);
            }

            //apply customer changes for filter
            FixFilter(request);
            //get filter params
            IEnumerable<IFilterItem> filterItemList = request?.GetFilterItemList();
            bool hasFilter = filterItemList != null;
            //apply filters from request
            if (hasFilter)
            {
                query = query.ApplyFilter(filterItemList);
            }
            //apply customer filters
            customerFilterList.ForEach(customFilter => query = customFilter(query));

            return query;
        }
        private (IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TModel>>) GetQueryAndSelect<TModel>() where TModel : class
        {
            //get the key to find the query and select expression for a certain model
            string modelKey = typeof(TModel).Name;
            //get query and select for a certain model
            IQueryable<TEntity> query = queryStore.ContainsKey(modelKey) ? queryStore[modelKey] : null;
            Func<IQueryable<TEntity>, IQueryable<TModel>> select = selectStore.ContainsKey(modelKey) ? selectStore[modelKey] as Func<IQueryable<TEntity>, IQueryable<TModel>> : null;

            return (query, select);
        }
        private void SetEntityToDelete(IKey<TKey> keyProvider)
        {
            if (keyProvider == null)
            {
                throw new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull);
            }
            //get the key
            TKey primaryKey = keyProvider.Key;
            Expression<Func<TEntity, bool>> predicate = PredicateProvider.GetPredicateForEntity<TEntity, TKey>(primaryKey);
            TEntity entity = DbSet.FirstOrDefault(predicate);
            if (entity == null)
            {
                return;
            }
            EntityEntry entry = Context.Entry(entity);
            entry.State = EntityState.Deleted;
        }
        #endregion
    }
}

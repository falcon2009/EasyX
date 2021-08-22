using AutoMapper;
using EasyX.Crud.Api.Data;
using EasyX.Crud.Api.Model;
using EasyX.Data.Api.Entity;
using EasyX.Data.Api.Request;
using EasyX.Data.Core.Request;
using EasyX.Infra;
using EasyX.Infra.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyX.ModelService
{
    public abstract class ModelServiceEFRepoGeneric<TEntity, TKey> : IModelService<TKey> where TEntity: class, IKey<TKey> where TKey : class
    {
        private const string GET_MODEL_ASYNC = "GetModelAsync";
        private const string GET_MODEL_LIST_ASYNC = "GetModelListAsync";
        private const string GET_TOTAL_ASYNC = "GetTotalAsync";
        private const string GET_FIRST_FROM_MODEL_LIST_ASYNC = "GetFirstFromModelAsync";

        protected IQueryableDataProvider<TEntity, TKey> DataProvider { get; private set; }
        private readonly IDataManager<TEntity, TKey> dataManager;
        private readonly IMapper mapper;
        private readonly Type dataProviderType;
        private readonly Dictionary<Type, Type> typeStorage = new();
        protected ModelServiceEFRepoGeneric(IQueryableDataProvider<TEntity, TKey> dataProvider, IDataManager<TEntity, TKey> dataManager, IMapper mapper)
        {
            this.DataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider), Constant.Errors.DataProviderNull);
            this.dataManager = dataManager ?? throw new ArgumentNullException(nameof(dataManager), Constant.Errors.DataManagerNull);
            this.mapper = mapper;
            dataProviderType = dataProvider.GetType();
            Initialize();
        }
        public virtual async Task<dynamic> GetModelAsync(string modelName, IKey<TKey> keyProvider, CancellationToken cancellationToken = default)
        {
            //check keyProvider
            if (keyProvider == null)
            {
                throw new ReadException(modelName, new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull)); ;
            }

            //get requested model type
            Type resultModelType = GetModelType(modelName);
            if (resultModelType == null)
            {
                throw new ReadException(modelName, new ArgumentException(Constant.Errors.UnsupportedModel));
            }

            try
            {
                //invoke method to get model
                dynamic task = dataProviderType.GetMethod(GET_MODEL_ASYNC)
                                               .MakeGenericMethod(typeStorage[resultModelType])
                                               .Invoke(DataProvider, new object[] { keyProvider, false, cancellationToken });
                object entityModel = await task.ConfigureAwait(false);

                //map data from entity model to result model
                return mapper.Map(entityModel, typeStorage[resultModelType], resultModelType);
            }
            catch (Exception exception)
            {
                throw new ReadException(modelName, exception);
            }
        }
        public virtual async Task<dynamic> GetModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            //check request
            if (request == null)
            {
                throw new ReadException($"List<{modelName}>", new ArgumentNullException(nameof(request), Constant.Errors.RequestFilterNull));
            }

            // make adjustments for request;
            FixRequest(request);

            //get requested model type
            Type resultModelType = GetModelType(modelName);
            if (resultModelType == null)
            {
                throw new ReadException($"List<{modelName}>", new ArgumentException(Constant.Errors.UnsupportedModel));
            }

            //get entity model list
            try
            {
                dynamic listTask = dataProviderType.GetMethod(GET_MODEL_LIST_ASYNC)
                                               .MakeGenericMethod(typeStorage[resultModelType])
                                               .Invoke(DataProvider, new object[] { request, cancellationToken });

                object entityModelList = await listTask.ConfigureAwait(false);

                //map data from entity model list to result model list
                object resultList = mapper.Map(entityModelList, typeof(List<>).MakeGenericType(typeStorage[resultModelType]), typeof(List<>).MakeGenericType(resultModelType));

                return resultList;
            }
            catch (Exception exception)
            {
                throw new ReadException($"List<{modelName}>", exception);
            }
        }
        public virtual async Task<dynamic> GetFirstFromModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            //check request
            if (request == null)
            {
                throw new ReadException($"List<{modelName}>", new ArgumentNullException(nameof(request), Constant.Errors.RequestFilterNull));
            }
            //get requested model type
            Type resultModelType = GetModelType(modelName);
            if (resultModelType == null)
            {
                throw new ReadException(modelName, new ArgumentException(Constant.Errors.UnsupportedModel));
            }

            // make adjustments for request;
            FixRequest(request);
            try
            {
                //invoke method to get model
                dynamic task = dataProviderType.GetMethod(GET_FIRST_FROM_MODEL_LIST_ASYNC)
                                               .MakeGenericMethod(typeStorage[resultModelType])
                                               .Invoke(DataProvider, new object[] { request, cancellationToken });
                object entityModel = await task.ConfigureAwait(false);

                //map data from entity model to result model
                return mapper.Map(entityModel, typeStorage[resultModelType], resultModelType);
            }
            catch (Exception exception)
            {
                throw new ReadException(modelName, exception);
            }
        }
        public virtual async Task<ITotalModel> GetTotalAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            string resultModelName = $"{nameof(TotalModel)} for {modelName}";
            //check filter
            if (request == null)
            {
                throw new ReadException($"{nameof(TotalModel)} for {modelName}", new ArgumentNullException(nameof(request), Constant.Errors.RequestFilterNull));
            }

            //get type for model
            Type resultModelType = GetModelType(modelName);
            if (resultModelType == null)
            {
                throw new ReadException($"{nameof(TotalModel)} for {modelName}", new ArgumentException(Constant.Errors.UnsupportedModel));
            }

            // make adjustments for request;
            FixRequest(request);
            try
            {
                //get total model
                Task<int> task = dataProviderType.GetMethod(GET_TOTAL_ASYNC)
                                                 .MakeGenericMethod(typeStorage[resultModelType])
                                                 .Invoke(DataProvider, new object[] { request, cancellationToken }) as Task<int>;

                return new TotalModel { Total = await task.ConfigureAwait(false) };
            }
            catch (Exception exception)
            {
                ReadException readException = new (resultModelName, exception);

                throw readException;
            }
        }
        public virtual async Task<TModel> InsertModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>
        {
            //check model
            if (model == null)
            {
                 throw new CreateException(typeof(TModel).Name, new ArgumentNullException(nameof(model), Constant.Errors.ModelNull));
            }

            try
            {
                //map data from model to entity
                TEntity entity = mapper.Map<TEntity>(model);
                //insert entity
                int result = await dataManager.InsertAsync(entity, cancellationToken).ConfigureAwait(false);
                if (result < 0)
                {
                    throw new CreateException(typeof(TModel).Name);
                }

                //map data from entity to model
                mapper.Map(entity, model);

                return model;
            }
            catch (Exception exception)
            {
                CreateException createException = new (typeof(TModel).Name, exception);

                throw createException;
            }


        }
        public virtual async Task<TModel> UpdateModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>
        {
            //check model
            if (model == null)
            {
                throw new UpdateException(typeof(TModel).Name, new ArgumentNullException(nameof(model), Constant.Errors.ModelNull)); 
            }

            try
            {
                //get entity from repository
                TEntity entity = await DataProvider.GetModelAsync<TEntity>(model, true, cancellationToken);

                //map data from model to entity;
                mapper.Map(model, entity);
                //update entity
                int result = await dataManager.UpdateAsync(entity, cancellationToken);
                if (result < 0)
                {
                    throw new UpdateException(nameof(TModel));
                }

                //map data from entity to model
                mapper.Map(entity, model);

                return model;
            }
            catch (Exception exception)
            {
                throw new UpdateException(nameof(TModel), exception);
            }
        }
        public virtual async Task<IDeleteModel> DeleteAsync(IKey<TKey> keyProvider, CancellationToken cancellationToken = default)
        {
            const string resultModelName = nameof(TEntity);
            //check keyProvider
            if (keyProvider == null)
            {
                throw new DeleteException(resultModelName, new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull)); ;
            }

            int result;
            try
            {
                //delete
                result = await dataManager.DeleteAsync(keyProvider, cancellationToken);
            }
            catch (Exception exception)
            {
                throw new DeleteException(resultModelName, exception); ;
            }

            return new DeleteModel() { IsDeleted = result > 0 };
        }

        #region protected
        protected abstract void ConfigureService();
        protected virtual void FixRequest(IRequest resuest)
        { }
        protected void BindModel<TModel, TEntityModel>() where TModel : class where TEntityModel :class
        {
            Type modelType = typeof(TModel);
            Type entityModelType = typeof(TEntityModel);
            if (!typeStorage.ContainsKey(modelType))
            {
                typeStorage.Add(modelType, entityModelType);
            }
        }
        #endregion

        #region private
        private void Initialize()
        {
            ConfigureService();
        }
        private Type GetModelType(string modelName)
        {
            return typeStorage.Keys.FirstOrDefault(item => item.Name == modelName);
        }
        #endregion
    }
}

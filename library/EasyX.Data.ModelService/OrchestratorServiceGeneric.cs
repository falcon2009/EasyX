using EasyX.Crud.Api.Model;
using EasyX.Data.Api.Entity;
using EasyX.Data.Api.Request;
using EasyX.Infra.Api;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EasyX.ModelService
{
    public abstract class OrchestratorServiceGeneric<TKey> : IOrchestratorService<TKey> where TKey : class
    {
        protected ITypeResolver TypeResolver { get; private set; }
        protected Dictionary<string, Func<string, IKey<TKey>, CancellationToken, Task<dynamic>>> GetModelDataFlow = new ();
        protected Dictionary<string, Func<string, IRequest, CancellationToken, Task<dynamic>>> GetModelListDataFlow = new ();
        protected Dictionary<string, Func<string, IRequest, CancellationToken, Task<dynamic>>> GetFirstModelDataFlow = new();
        protected Dictionary<string, Func<string, IRequest, CancellationToken, Task<ITotalModel>>> GetTotalModelDataFlow = new();
        protected Dictionary<string, Func<object, CancellationToken, Task<object>>> InsertDataFlow = new();
        protected Dictionary<string, Func<object, CancellationToken, Task<object>>> UpdateDataFlow = new();
        protected OrchestratorServiceGeneric(ITypeResolver typeResolver)
        {
            TypeResolver = typeResolver;
            Initialize();
        }

        public virtual async Task<dynamic> GetModelAsync(string modelName, IKey<TKey> keyProvider, CancellationToken cancellationToken = default)
        {
            Func<string, IKey<TKey>, CancellationToken, Task<dynamic>> task = GetModelDataFlow.GetValueOrDefault(modelName);

            return await task(modelName, keyProvider, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<dynamic> GetModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            Func<string, IRequest, CancellationToken, Task<dynamic>> task = GetModelListDataFlow.GetValueOrDefault(modelName);

            return await task(modelName, request, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<dynamic> GetFirstFromModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            Func<string, IRequest, CancellationToken, Task<dynamic>> task = GetFirstModelDataFlow.GetValueOrDefault(modelName);

            return await task(modelName, request, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<ITotalModel> GetTotalAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            Func<string, IRequest, CancellationToken, Task<ITotalModel>> task = GetTotalModelDataFlow.GetValueOrDefault(modelName);

            return await task(modelName, request, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task<TModel> InsertModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>
        {
            Func<object, CancellationToken, Task<object>> task = InsertDataFlow.GetValueOrDefault(nameof(TModel));

            return await task(model, cancellationToken).ConfigureAwait(false) as TModel;
        }

        public virtual async Task<TModel> UpdateModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>
        {
            Func<object, CancellationToken, Task<object>> task = UpdateDataFlow.GetValueOrDefault(nameof(TModel));

            return await task(model, cancellationToken).ConfigureAwait(false) as TModel;
        }

        public virtual Task<IDeleteModel> DeleteAsync(IKey<TKey> keyProvider, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #region protected
        protected abstract void ConfigureService();
        #endregion

        #region private
        private void Initialize()
        {
            ConfigureService();
        }
        #endregion
    }
}
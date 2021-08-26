using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using EasyX.Crud.Api.Model;
using EasyX.Http;
using EasyX.Data.Api.Entity;
using EasyX.Infra.Exception;
using EasyX.Infra;
using EasyX.Data.Api.Request;
using EasyX.Data.Core.Extention;
using EasyX.Data.Core.Request;
using EasyX.Infra.Api;

namespace ServiceCore.Services
{
    public abstract class ModelServiceHttpGeneric<TKey> : IModelService<TKey> where TKey : class
    {
        private const string GET_ASYNC = "GetAsync";
        private readonly ITypeResolver typeResolver;
        private readonly IHttpService httpService;
        private readonly Type contextType;
        protected HttpClient HttpClient { get; private set; }
        protected abstract string Controller { get;}

        protected ModelServiceHttpGeneric(IHttpService httpService, HttpClient httpClient, ITypeResolver typeResolver)
        {
            this.typeResolver = typeResolver ?? throw new ArgumentNullException(nameof(httpClient), "TypeResolver cannot be null");
            this.httpService = httpService ?? throw new ArgumentNullException(nameof(httpClient), "HttpService cannot be null");
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient), "HttpClient cannot be null");
            this.contextType = httpService.GetType();
        }
        public virtual async Task<dynamic> GetModelAsync(string modelName, IKey<TKey> keyProvider, CancellationToken cancellationToken = default)
        {
            //check keyProvider
            if (keyProvider == null)
            {
                throw new ReadException(modelName, new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull));
            }

            //get requested model type
            Type resultType = typeResolver.GetType(modelName);
            if (resultType == null)
            {
                throw new ReadException(modelName, new ArgumentException(Constant.Errors.UnsupportedModel));
            }

            try
            {
                //invoke  getasync with arguments
                dynamic task = contextType.GetMethod(GET_ASYNC, new Type[] { typeof(HttpClient), typeof(Uri), typeof(CancellationToken), typeof(string) })
                                          .MakeGenericMethod(resultType)
                                          .Invoke(HttpClient, new object[] { HttpClient, new Uri($"{Controller}/{modelName}/{keyProvider.ToQuery()}", UriKind.Relative), cancellationToken, Constant.MediaType.Application.Json });
                
                return await task.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new ReadException(modelName, exception);
            }
        }
        public virtual async Task<dynamic> GetModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            //check filter
            if (request == null)
            {
                throw new ReadException(modelName, new ArgumentNullException(nameof(request), Constant.Errors.RequestFilterNull));
            }

            //get requested model type
            Type resultType = typeResolver.GetType(modelName);
            if (resultType == null)
            {
                throw new ReadException(modelName, new ArgumentException(Constant.Errors.UnsupportedModel)); ;
            }

            try
            {
                dynamic task = contextType.GetMethod(GET_ASYNC, new Type[] { typeof(HttpClient), typeof(Uri), typeof(CancellationToken), typeof(string) })
                                          .MakeGenericMethod(typeof(List<>).MakeGenericType(resultType))
                                          .Invoke(httpService, new object[] { HttpClient, new Uri($"{Controller}/lists/{modelName}?{request.ToQuery()}", UriKind.Relative), cancellationToken, Constant.MediaType.Application.Json });

                return await task.ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new ReadException(modelName, exception);
            }
        }
        public virtual async Task<ITotalModel> GetTotalAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            //check filter
            if (request == null)
            {
                throw new ReadException($"{nameof(TotalModel)} for {modelName}", new ArgumentNullException(nameof(request), Constant.Errors.RequestFilterNull));
            }

            return await httpService.GetAsync<TotalModel>(HttpClient, new Uri($"{Controller}/total/{modelName}?{request.ToQuery()}", UriKind.Relative), cancellationToken)
                                    .ConfigureAwait(false);
        }
        public virtual async Task<TModel> InsertModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>
        {
            //check model
            if (model == null)
            {
                throw new CreateException(nameof(TModel), new ArgumentNullException(nameof(model), Constant.Errors.ModelNull));
            }

            return await httpService.PostAsync<TModel, TModel>(HttpClient, new Uri(Controller, UriKind.Relative), model, cancellationToken)
                                    .ConfigureAwait(false);
        }
        public virtual async Task<TModel> UpdateModelAsync<TModel>(TModel model, CancellationToken cancellationToken = default) where TModel : class, IKey<TKey>
        {
            //check model
            if (model == null)
            {
                throw new UpdateException(nameof(TModel), new ArgumentNullException(nameof(model), Constant.Errors.ModelNull));
            }

            return await httpService.PostAsync<TModel, TModel>(HttpClient, new Uri(Controller, UriKind.Relative), model, cancellationToken)
                                    .ConfigureAwait(false);
        }
        public virtual async Task<IDeleteModel> DeleteAsync(IKey<TKey> keyProvider, CancellationToken cancellationToken = default)
        {
            //check keyProvider
            if (keyProvider == null)
            {
                throw new DeleteException(Controller, new ArgumentNullException(nameof(keyProvider), Constant.Errors.KeyNull)); ;
            }
            //convert key to query string
            try
            {
                return await httpService.DeleteAsync<DeleteModel>(HttpClient, new Uri($"{Controller}/{keyProvider.ToQuery()}", UriKind.Relative), cancellationToken)
                                        .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                throw new DeleteException(Controller, exception);
            }
        }
        public Task<dynamic> GetFirstFromModelListAsync(string modelName, IRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

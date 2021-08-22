using EasyX.Infra;
using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EasyX.Http
{
    public interface IHttpService
    {
        Task<TResult> GetAsync<TResult>(HttpClient httpClient, Uri uri, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class;
        Task<TResult> PostAsync<T, TResult>(HttpClient httpClient, Uri uri, T item, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class;
        Task<TResult> PutAsync<T, TResult>(HttpClient httpClient, Uri uri, T item, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class;
        Task<TResult> DeleteAsync<TResult>(HttpClient httpClient, Uri uri, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class;
        void AddHearder(string key, string value);
        void SetAuthorization(string authorizationToken, string authorizationMethod = "Bearer");
        void SetJsonSerializerOptions(JsonSerializerOptions options);
    }
}

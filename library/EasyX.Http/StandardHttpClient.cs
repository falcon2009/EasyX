using EasyX.Infra;
using EasyX.Infra.Exception;
using EasyX.Infra.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EasyX.Http
{
    public class StandardHttpClient : IHttpService
    {

        private readonly IHttpContextAccessor httpContextAccessor;
        private JsonSerializerOptions jsonOption;
        private readonly Dictionary<string, string> headerStorage = new Dictionary<string, string>();
        private string authorizationToken;
        private string authorizationMethod;

        public StandardHttpClient(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            jsonOption = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }
        public async Task<TResult> GetAsync<TResult>(HttpClient httpClient, Uri uri, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class
        {
            //create request
            using HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            using HttpResponseMessage responseMessage = await SendRequestBaseAsync(httpClient, requestMessage, mediaType, cancellationToken).ConfigureAwait(false);
            return await GetResultFromResponseAsync<TResult>(responseMessage, mediaType, cancellationToken).ConfigureAwait(false);

        }
        public async Task<TResult> PostAsync<T, TResult>(HttpClient httpClient, Uri uri, T item, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class
        {
            using HttpResponseMessage responseMessage = await SendRequestWithContentAsync(httpClient, uri, HttpMethod.Post, item, mediaType, cancellationToken).ConfigureAwait(false);
            return await GetResultFromResponseAsync<TResult>(responseMessage, mediaType, cancellationToken).ConfigureAwait(false);
        }
        public async Task<TResult> PutAsync<T, TResult>(HttpClient httpClient, Uri uri, T item, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class
        {
            using HttpResponseMessage responseMessage = await SendRequestWithContentAsync(httpClient, uri, HttpMethod.Put, item, mediaType, cancellationToken).ConfigureAwait(false);
            return await GetResultFromResponseAsync<TResult>(responseMessage, mediaType, cancellationToken).ConfigureAwait(false);
        }
        public async Task<TResult> DeleteAsync<TResult>(HttpClient httpClient, Uri uri, CancellationToken cancellationToken = default, string mediaType = Constant.MediaType.Application.Json) where TResult : class
        {
            //send request
            using HttpResponseMessage responseMessage = await SendRequestWithoutContentAsync(httpClient, uri, HttpMethod.Delete, mediaType, cancellationToken).ConfigureAwait(false);
            return await GetResultFromResponseAsync<TResult>(responseMessage, mediaType, cancellationToken).ConfigureAwait(false);
        }
        public void AddHearder(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            bool hasHeader = headerStorage.ContainsKey(key);
            if (hasHeader)
            {
                return;
            }
            headerStorage.Add(key, value);
        }
        public void SetJsonSerializerOptions(JsonSerializerOptions options)
        {
            jsonOption = options;
        }
        public void SetAuthorization(string authorizationToken, string authorizationMethod = "Bearer")
        {
            this.authorizationToken = authorizationToken;
            this.authorizationMethod = authorizationMethod;
        }

        #region protected
        protected virtual async Task<TResult> GetResultFromResponseAsync<TResult>(HttpResponseMessage responseMessage, string mediaType, CancellationToken cancellationToken) where TResult : class
        {
            if (typeof(TResult) == typeof(Attachment))
            {
                Stream stream = await responseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                return new Attachment(stream, responseMessage.Content.Headers.ContentDisposition.FileName, responseMessage.Content.Headers.ContentType.MediaType) as TResult;
            }

            string content = await responseMessage.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            if (typeof(TResult) == typeof(string))
            {
                return content as TResult;
            }

            if (mediaType == Constant.MediaType.Application.Xml)
            {
                return content.DeserializeXml<TResult>();
            }

            return content.DeserializeJson<TResult>(jsonOption);
        }
        protected virtual HttpRequestMessage GetRequestMessageWithBody<T>(Uri uri, HttpMethod method, string mediaType, T item)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(method, uri);
            if (item == null)
            {
                return requestMessage;
            }

            if (mediaType == Constant.MediaType.Application.Json)
            {
                string json = JsonSerializer.Serialize(item, jsonOption);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, mediaType);

                return requestMessage;
            }

            if (mediaType == Constant.MediaType.Application.Xml)
            {
                requestMessage.Content = new StringContent(item as string, Encoding.UTF8, mediaType);

                return requestMessage;
            }

            if (item is Attachment attachment)
            {
                StreamContent content = new (attachment.ContentStream);
                content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                MultipartFormDataContent multipartContent = new();
                multipartContent.Add(content, "file", attachment.Name);
                requestMessage.Content = multipartContent;

                return requestMessage;
            }

            if (item is IEnumerable<Attachment> attachmentList)
            {
                var multipartContent = new MultipartFormDataContent();

                foreach (var attach in attachmentList)
                {
                    StreamContent content = new(attach.ContentStream);
                    content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
                    multipartContent.Add(content, "files", attach.Name);
                }

                requestMessage.Content = multipartContent;
            }
            return requestMessage;
        }
        #endregion

        #region private
        private  async Task<HttpResponseMessage> SendRequestWithoutContentAsync(HttpClient httpClient, Uri uri, HttpMethod method, string mediaType, CancellationToken cancellationToken)
        {
            //create request
            using HttpRequestMessage requestMessage = new HttpRequestMessage(method, uri);
            //send request
            return await SendRequestBaseAsync(httpClient, requestMessage, mediaType, cancellationToken);
        }
        private async Task<HttpResponseMessage> SendRequestWithContentAsync<T>(HttpClient httpClient, Uri uri, HttpMethod method, T item, string mediaType, CancellationToken cancellationToken)
        {
            //check metod type
            if (method != HttpMethod.Post || method != HttpMethod.Put)
            {
                throw new ArgumentException("Value must be either post or put.", nameof(method));
            }

            //create request
            using HttpRequestMessage requestMessage = GetRequestMessageWithBody<T>(uri, method, mediaType, item);

            string json = JsonSerializer.Serialize(item, jsonOption);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, mediaType);
            //send request
            return await SendRequestBaseAsync(httpClient, requestMessage, mediaType, cancellationToken).ConfigureAwait(false);
        }
        private async Task<HttpResponseMessage> SendRequestBaseAsync(HttpClient httpClient, HttpRequestMessage requestMessage, string mediaType, CancellationToken cancellationToken)
        {
            SetAuthorizationHeader(requestMessage);
            SetCustomerHeader(requestMessage, mediaType);
            HttpResponseMessage responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken).ConfigureAwait(false);
            await CheckResponseForErrorAsync(responseMessage).ConfigureAwait(false);

            return responseMessage;
        }
        private void SetAuthorizationHeader(HttpRequestMessage requestMessage)
        {
            bool shouldSetAuthorizationHeader = !string.IsNullOrEmpty(authorizationToken);

            if (shouldSetAuthorizationHeader)
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }
            else
            {
                StringValues authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"] ?? string.Empty;
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    requestMessage.Headers.Add("Authorization", new List<string> { authorizationHeader });
                }
            }
        }
        private void SetCustomerHeader(HttpRequestMessage requestMessage, string mediaType)
        {
            requestMessage.Headers.Add("Accept", mediaType);

            foreach (string key in headerStorage.Keys)
            {
                if (!requestMessage.Headers.Contains(key))
                {
                    string value = headerStorage[key];
                    requestMessage.Headers.Add(key, value);
                }
            }
        }
        private async Task CheckResponseForErrorAsync(HttpResponseMessage responseMessage)
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                return;
            }

            string result = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (string.IsNullOrEmpty(result))
            {
                throw new StatusCodeException(responseMessage.StatusCode, $"{responseMessage.ReasonPhrase}- Method: {responseMessage.RequestMessage.Method}, Path: {responseMessage.RequestMessage.RequestUri}");
            }

            try
            {
                ErrorModel errorModel = JsonSerializer.Deserialize<ErrorModel>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (!string.IsNullOrEmpty(errorModel.Message))
                {
                    throw new StatusCodeException(responseMessage.StatusCode, errorModel.Message);
                }
            }
            catch (JsonException)
            {
                throw new StatusCodeException(responseMessage.StatusCode, result);
            }
        }
        #endregion
    }
}


using EasyX.Infra;
using EasyX.Infra.Exception;
using EasyX.Infra.Extention;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;


namespace EasyX.WebApi.Middleware
{
    public class JsonExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JsonSerializerOptions serializeOption;
        private readonly ILogger logger;

        public JsonExceptionMiddleware(RequestDelegate next, ILogger<JsonExceptionMiddleware> logger)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            serializeOption = new()
            {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true
            };
            this.logger = logger;
        }

        public JsonExceptionMiddleware(RequestDelegate next, JsonSerializerOptions serializeOption)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.serializeOption = serializeOption;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (StatusCodeException exception)
            {
                SetContextResponseStatusCode(context, exception.StatusCode);
                ErrorModel model = GetErrroModel(exception, context);
                await WriteJsonExceptionAsync(context, model).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                SetContextResponseStatusCode(context, HttpStatusCode.InternalServerError);
                ErrorModel model = GetErrroModel(exception, context);
                await WriteJsonExceptionAsync(context, model).ConfigureAwait(false);
            }
        }

        #region private
        private static void SetContextResponseStatusCode(HttpContext context, HttpStatusCode statusCode)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Response.StatusCode = (int)statusCode;
        }
        private ErrorModel GetErrroModel(Exception exeption, HttpContext contex)
        {
            string logMessage = string.Empty;
            if (contex != null)
            {
                string path = contex.Request.Path.Value;
                string method = contex.Request.Method;
                Guid id = Guid.NewGuid();
                logMessage = $"ID={id}, controller={path}, method={method}";
            }

            logger?.LogError(exeption, logMessage);
            string details = exeption.StackTrace;
            string message = exeption.ToStringWithInnerDetails();
            ErrorModel model = new()
            {
                Details = details,
                Message = message
            };

            return model;
        }
        private async Task WriteJsonExceptionAsync(HttpContext context, ErrorModel model)
        {
            context.Response.ContentType = Constant.MediaType.Application.Json;
            await JsonSerializer.SerializeAsync(context.Response.Body, model, serializeOption).ConfigureAwait(false);
            await context.Response.CompleteAsync().ConfigureAwait(false);
        }
        #endregion`
    }
}

using EasyX.Infra;
using EasyX.WebApi.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace EasyX.WebApi.Middleware
{
    public class TransactionIdMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<TransactionIdMiddleware> logger;
        public TransactionIdMiddleware(RequestDelegate next, ILogger<TransactionIdMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context, DiagnosticContext diagnosticContext)
        {
            DateTime startedOn = DateTime.UtcNow;
            string transactionId = string.Empty;
            bool isSwaggerRequest = context.Request.Path.Value.Contains("swagger", StringComparison.InvariantCultureIgnoreCase);
            if (!isSwaggerRequest)
            {

                transactionId = logger.LogStartTransaction(startedOn);
                context.Request.Headers.Add(Constant.Headers.Transaction, transactionId);
            }

            await next(context);
            if (!isSwaggerRequest)
            {
                logger.LogEndTransaction(transactionId, startedOn);
            }
        }
    }
}

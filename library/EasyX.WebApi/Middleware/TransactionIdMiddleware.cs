using EasyX.Infra;
using EasyX.Infra.Extension;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace EasyX.WebApi.Middleware
{
    public class TransactionIdMiddleware
    {
        private readonly RequestDelegate next;
        public TransactionIdMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(Constant.Headers.Transaction))
            {
                context.Request.Headers.Add(Constant.Headers.Transaction, Guid.NewGuid().ToNSting());
            }

            await next(context);
        }
    }
}

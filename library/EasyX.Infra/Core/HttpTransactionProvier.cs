using EasyX.Infra.Api;
using Microsoft.AspNetCore.Http;

namespace EasyX.Infra.Core
{
    public class HttpTransactionProvier : ITransactionProvider
    {
        public string TransactionId { get; private set; }
        public HttpTransactionProvier(IHttpContextAccessor httpContextAccessor)
        {
            TransactionId = httpContextAccessor.HttpContext.Request.Headers[Constant.Headers.Transaction];
        }
    }
}

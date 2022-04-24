using System;

namespace EasyX.Infra.Api
{
    public interface ITransactionProvider
    {
        public string TransactionId { get; }
    }
}

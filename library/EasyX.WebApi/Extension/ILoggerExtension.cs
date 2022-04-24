using EasyX.Infra.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using Serilog.Extensions.Hosting;
using Serilog.Parsing;
using System;
using System.Collections.Generic;

namespace EasyX.WebApi.Extension
{
    public static class ILoggerExtension
    {
        private static readonly string startTransactionMessage = "Start transaction - Id: {TransactionId}, started on: {StartedOn},";
        private static readonly string endTransactionMessage = "Finish transaction - Id: {TransactionId}, Elapsed: {Elapsed},";
        public static string LogStartTransaction(this ILogger logger, DateTime startedOn)
        {
            string transactionId = Guid.NewGuid().ToNSting();
            logger.LogInformation(startTransactionMessage, transactionId, startedOn);

            return transactionId;
        }

        public static void LogEndTransaction(this ILogger logger, string transactionId, DateTime startedOn)
        {
            logger.LogInformation(endTransactionMessage, transactionId, DateTime.UtcNow.Subtract(startedOn));
        }
    }
}

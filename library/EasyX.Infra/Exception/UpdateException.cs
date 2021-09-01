using System;
using System.Net;
using System.Runtime.Serialization;

namespace EasyX.Infra.Exception
{
#nullable enable
    [Serializable]
    public class UpdateException : StatusCodeException
    {
        public UpdateException()
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        public UpdateException(string modelName) : base(HttpStatusCode.Conflict, $"Cannot update instace of {modelName}.")
        {
        }
        public UpdateException(string modelName, System.Exception? innerException) : base(HttpStatusCode.Conflict, $"Cannot update instace of {modelName}.", innerException)
        {
        }
        protected UpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
    }
#nullable disable
}

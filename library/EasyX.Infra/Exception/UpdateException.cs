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
        public UpdateException(string modelName) : base($"Cannot update instace of {modelName}.")
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        public UpdateException(string modelName, System.Exception? innerException) : base($"Cannot update instace of {modelName}.", innerException)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        protected UpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
    }
#nullable disable
}

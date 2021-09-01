using System;
using System.Net;
using System.Runtime.Serialization;

namespace EasyX.Infra.Exception
{
#nullable enable
    [Serializable]
    public class CreateException : StatusCodeException
    {
        public CreateException()
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        public CreateException(string modelName) : base(HttpStatusCode.Conflict, $"Cannot insert instace of {modelName}.")
        {
        }
        public CreateException(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {
        }
        public CreateException(string modelName, System.Exception? innerException) : base(HttpStatusCode.Conflict, $"Cannot insert instace of {modelName}.", innerException)
        {
        }
        public CreateException(string modelName, StatusCodeException innerException) : base(innerException.StatusCode, $"Cannot insert instace of {modelName}.", innerException)
        {
        }
        protected CreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
    }
#nullable disable
}

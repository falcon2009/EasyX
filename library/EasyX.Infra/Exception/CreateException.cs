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
        public CreateException(string modelName) : base($"Cannot insert instace of {modelName}.")
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        public CreateException(string modelName, System.Exception? innerException) : base($"Cannot insert instace of {modelName}.", innerException)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        protected CreateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
    }
#nullable disable
}

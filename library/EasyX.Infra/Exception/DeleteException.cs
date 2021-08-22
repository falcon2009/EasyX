using System;
using System.Net;
using System.Runtime.Serialization;

namespace EasyX.Infra.Exception
{
#nullable enable
    [Serializable]
    public class DeleteException : StatusCodeException
    {
        public DeleteException()
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        public DeleteException(string modelName) : base($"Cannot delete instace of {modelName}.")
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        public DeleteException(string modelName, System.Exception? innerException) : base($"Cannot delete instace of {modelName}.", innerException)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
        protected DeleteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.Conflict;
        }
    }
#nullable disable
}

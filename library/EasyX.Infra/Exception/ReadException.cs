using System;
using System.Net;
using System.Runtime.Serialization;

namespace EasyX.Infra.Exception
{
#nullable enable
    [Serializable]
    public class ReadException : StatusCodeException
    {
        public ReadException()
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
        public ReadException(string modelName, System.Exception? innerException) : base(HttpStatusCode.BadRequest, $"Cannot get {modelName}.", innerException)
        {
        }
        public ReadException(string modelName, StatusCodeException innerException) : base(innerException.StatusCode, $"Cannot get {modelName}.", innerException)
        {
        }
        protected ReadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
#nullable disable
}

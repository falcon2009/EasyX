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
        public ReadException(string modelName) : base($"Cannot get {modelName}.")
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
        public ReadException(string modelName, System.Exception? innerException) : base($"Cannot get {modelName}.", innerException)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
        protected ReadException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
#nullable disable
}

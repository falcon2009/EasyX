using System;
using System.Net;
using System.Runtime.Serialization;

namespace EasyX.Infra.Exception
{
#nullable enable
    [Serializable]
    public class StatusCodeException : System.Exception
    {
        protected HttpStatusCode StatusCode { get; set; }
        public StatusCodeException()
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
        public StatusCodeException(string message) : base(message)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
        public StatusCodeException(string message, System.Exception? innerException) : base(message, innerException)
        {
            StatusCode = HttpStatusCode.InternalServerError;
        }
        public StatusCodeException(HttpStatusCode StatusCode, string message) : base(message)
        {
            this.StatusCode = StatusCode;
        }
        protected StatusCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
#nullable disable
    }
}

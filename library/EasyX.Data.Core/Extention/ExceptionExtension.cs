using System;
using System.Text;

namespace EasyX.Data.Core.Extention
{
    public static class ExceptionExtension
    {
        public static string ToStringWithInnerDetails(this Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            StringBuilder strBuilder = new StringBuilder(exception.Message);
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                strBuilder.Append(" ==> ");
                strBuilder.Append(innerException.Message);
                innerException = innerException.InnerException;
            }
            return strBuilder.ToString();
        }
    }
}

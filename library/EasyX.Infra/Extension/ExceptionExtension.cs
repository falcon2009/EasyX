using System.Text;

namespace EasyX.Infra.Extention
{
    public static class ExceptionExtension
    {
        public static string ToStringWithInnerDetails(this System.Exception exception)
        {
            if (exception == null)
            {
                return string.Empty;
            }

            StringBuilder strBuilder = new StringBuilder(exception.Message);
            System.Exception innerException = exception.InnerException;
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

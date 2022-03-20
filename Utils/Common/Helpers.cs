using System.Net;

namespace DesafioStone.Utils.Common
{
    public class Helpers
    {
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (typeof(T) == typeof(bool) && obj is sbyte)
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }

            if (obj == null || obj == DBNull.Value)
            {
                return default;
            }
            else
            {
                return (T)obj;
            }
        }

        public static HttpRequestException BuildHttpException(HttpStatusCode statusCode, string message)
        {
            return new HttpRequestException(message, null, statusCode);
        }
    }
}

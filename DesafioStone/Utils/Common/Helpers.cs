using System.Data;
using System.Net;
using System.Security.Cryptography;

namespace DesafioStone.Utils.Common
{
    public static class Helpers
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

        public static HttpRequestException BuildHttpException(HttpStatusCode statusCode, string message = null)
        {
            return new HttpRequestException(message, null, statusCode);
        }

        public static void AddWithValue<T>(this IDbCommand command, string name, T value)
        {
            IDbDataParameter parameter = command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

        public static string GenerateRandomToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}

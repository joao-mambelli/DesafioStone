using DesafioStone.DataContracts;
using DesafioStone.Interfaces.ModelsInterfaces;
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

        public static InvoicePatchRequest InvoiceToInvoicePatchRequest(IInvoice invoice)
        {
            return new InvoicePatchRequest()
            {
                ReferenceMonth = invoice.ReferenceMonth,
                ReferenceYear = invoice.ReferenceYear,
                Document = invoice.Document,
                Description = invoice.Description,
                Amount = invoice.Amount
            };
        }

        public static HttpRequestException BuildHttpException(HttpStatusCode statusCode, string message)
        {
            return new HttpRequestException(message, null, statusCode);
        }
    }
}

using DesafioStone.Enums;
using System.ComponentModel.DataAnnotations;
using DesafioStone.Models;

namespace DesafioStone.DataContracts
{
    public class InvoicePatchRequest
    {
        public InvoicePatchRequest()
        {
            
        }

        public InvoicePatchRequest(Invoice invoice)
        {
            ReferenceMonth = invoice.ReferenceMonth;
            ReferenceYear = invoice.ReferenceYear;
            Document = invoice.Document;
            Description = invoice.Description;
            Amount = invoice.Amount;
        }

        [EnumDataType(typeof(MonthEnum))]
        public MonthEnum ReferenceMonth { get; set; }

        [Range(1900, int.MaxValue)]
        public int ReferenceYear { get; set; }

        [RegularExpression(@"^[0-9]{11}$|^[0-9]{14}$")]
        public string Document { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
    }
}

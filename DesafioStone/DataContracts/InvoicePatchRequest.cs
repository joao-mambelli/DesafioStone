using DesafioStone.Enums;
using System.ComponentModel.DataAnnotations;
using DesafioStone.Models;
using DesafioStone.Validations;

namespace DesafioStone.DataContracts
{
    public class InvoicePatchRequest
    {
        public InvoicePatchRequest()
        {
            
        }

        // Constructor used to convert Invoice to InvoicePatchRequest.
        // Useful for the InvoiceService.PatchInvoiceAsync() method.
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

        [ValidDocument]
        public string Document { get; set; }

        [StringLength(256)]
        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Amount { get; set; }
    }
}

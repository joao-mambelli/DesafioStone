using DesafioStone.DataContracts;
using DesafioStone.Enums;

namespace DesafioStone.Models
{
    public class Invoice
    {
        public Invoice()
        {

        }

        public Invoice(InvoiceCreateRequest request)
        {
            ReferenceMonth = request.ReferenceMonth ?? MonthEnum.January;
            ReferenceYear = request.ReferenceYear ?? 0;
            Amount = request.Amount ?? 0;
            Description = request.Description;
            Document = request.Document;
        }

        public long Id { get; set; }
        public MonthEnum ReferenceMonth { get; set; }
        public int ReferenceYear { get; set; }
        public string Document { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeactivatedAt { get; set; }
    }
}
using DesafioStone.Enums;

namespace DesafioStone.Interfaces
{
    public interface IInvoice
    {
        long Id { get; set; }
        Month ReferenceMonth { get; set; }
        int ReferenceYear { get; set; }
        string Document { get; set; }
        string Description { get; set; }
        int Amount { get; set; }
        bool IsActive { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime? DeactivatedAt { get; set; }
    }
}
namespace DesafioStone.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int ReferenceMonth { get; set; }
        public int ReferenceYear { get; set; }
        public string? Document { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime DeactivatedAt { get; set; }
    }
}
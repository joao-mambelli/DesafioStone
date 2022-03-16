namespace DesafioStone.DataContracts
{
    public class InvoiceCreateRequest
    {
        public int ReferenceMonth { get; set; }
        public int ReferenceYear { get; set; }
        public string Document { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}

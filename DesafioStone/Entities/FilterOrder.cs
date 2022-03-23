using DesafioStone.Enums;

namespace DesafioStone.Entities
{
    public class FilterOrder
    {
        public InvoiceOrderFieldEnum Field { get; set; }
        public OrderDirectionEnum Direction { get; set; }
    }
}

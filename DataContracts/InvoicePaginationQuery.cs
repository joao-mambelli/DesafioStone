using System.ComponentModel.DataAnnotations;

namespace DesafioStone.DataContracts
{
    public class InvoicePaginationQuery
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int Page { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int RowsPerPage { get; set; }
    }
}

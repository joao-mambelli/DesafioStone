using DesafioStone.Enums;
using System.ComponentModel.DataAnnotations;
using DesafioStone.Validations;

namespace DesafioStone.DataContracts
{
    public class InvoiceUpdateRequest
    {
        [RequiredEnumField]
        public MonthEnum? ReferenceMonth { get; set; }

        [Required]
        [Range(1900, int.MaxValue)]
        public int? ReferenceYear { get; set; }

        [Required]
        [ValidDocument]
        public string Document { get; set; }

        [Required]
        [StringLength(256)]
        public string Description { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Amount { get; set; }
    }
}

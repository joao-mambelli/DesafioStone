using DesafioStone.Enums;
using System.ComponentModel.DataAnnotations;

namespace DesafioStone.DataContracts
{
    public class InvoicePatchRequest
    {
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

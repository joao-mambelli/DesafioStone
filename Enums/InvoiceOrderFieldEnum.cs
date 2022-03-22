using System.Runtime.Serialization;

namespace DesafioStone.Enums
{
    public enum InvoiceOrderFieldEnum
    {
        [EnumMember(Value = "year")]
        Year = 0,

        [EnumMember(Value = "month")]
        Month = 1,

        [EnumMember(Value = "document")]
        Document = 2,
    }
}

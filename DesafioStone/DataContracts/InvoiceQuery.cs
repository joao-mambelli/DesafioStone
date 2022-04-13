using System.ComponentModel.DataAnnotations;
using DesafioStone.Validations;
using DesafioStone.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace DesafioStone.DataContracts
{
    public class InvoiceQuery
    {
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, int.MaxValue)]
        public int RowsPerPage { get; set; } = int.MaxValue;

        [Range(1, 12)]
        public int? Month { get; set; }

        [Range(1900, int.MaxValue)]
        public int? Year { get; set; }

        [ValidDocument]
        public string Document { get; set; }

        [SwaggerParameter(Description = "Specify this parameter to order the invoices.\n\nYou can order by all fields in InvoiceOrderFieldEnum.\n\nIt will order by the same order of the array you will provide. Example:\n\n\t&orders[0].field=year&orders[1].field=month&orders[1].direction=descending\n\nThis will send an array of two objects. This array will look something like this:\n\n\t[\n\n\t\t{\n\n\t\t\tfield: \"year\",\n\n\t\t\tdirection: \"ascending\" //\"ascending\" is the default value\n\n\t\t},\n\n\t\t{\n\n\t\t\tfield: \"month\",\n\n\t\t\tdirection: \"descending\"\n\n\t\t}\n\n\t]\n\nThis will retrieve invoices ordered first by year in increasing fashion, and those with same year will be ordered by month in a decreasing order.")]
        public List<FilterOrder> Orders { get; set; }
    }
}

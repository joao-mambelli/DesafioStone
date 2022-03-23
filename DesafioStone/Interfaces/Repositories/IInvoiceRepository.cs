using DesafioStone.Models;
using DesafioStone.DataContracts;

namespace DesafioStone.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        IEnumerable<Invoice> GetInvoices(InvoiceQuery query, bool active = true);
        Invoice GetInvoiceById(long invoiceId, bool active = true);
        long GetInvoiceCount(bool active = true);
        long InsertInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(long invoiceId);
    }
}
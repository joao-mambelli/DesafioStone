using DesafioStone.Models;
using DesafioStone.DataContracts;

namespace DesafioStone.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetInvoicesAsync(InvoiceQuery query, bool active = true);
        Task<Invoice> GetInvoiceByIdAsync(long invoiceId, bool active = true);
        Task<long> GetInvoiceCountAsync(bool active = true);
        Task<long> InsertInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(long invoiceId);
    }
}
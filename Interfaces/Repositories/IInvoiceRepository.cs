using DesafioStone.Models;

namespace DesafioStone.Interfaces.Repositories
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync(bool active = true);
        Task<IEnumerable<Invoice>> GetInvoicesOffsetAsync(int offset, int amount, bool active = true);
        Task<Invoice> GetInvoiceByIdAsync(long invoiceId, bool active = true);
        Task<long> GetInvoiceCountAsync(bool active = true);
        Task<long> InsertInvoiceAsync(Invoice invoice);
        Task UpdateInvoiceAsync(Invoice invoice);
        Task DeleteInvoiceAsync(long invoiceId);
    }
}
using DesafioStone.Interfaces.ModelsInterfaces;

namespace DesafioStone.Interfaces.RepositoriesInterfaces
{
    public interface IInvoiceRepository
    {
        Task<IEnumerable<IInvoice>> GetAllInvoicesAsync(bool active = true);
        Task<IEnumerable<IInvoice>> GetInvoicesOffsetAsync(int offset, int amount, bool active = true);
        Task<IInvoice> GetInvoiceByIdAsync(long invoiceId, bool active = true);
        Task<long> GetInvoiceCountAsync(bool active = true);
        Task<long> InsertInvoiceAsync(IInvoice invoice);
        Task UpdateInvoiceAsync(IInvoice invoice);
        Task DeleteInvoiceAsync(long invoiceId);
    }
}
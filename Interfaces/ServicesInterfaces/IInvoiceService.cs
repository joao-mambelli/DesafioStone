using DesafioStone.DataContracts;
using DesafioStone.Interfaces.ModelsInterfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioStone.Interfaces.ServicesInterfaces
{
    public interface IInvoiceService
    {
        Task<IEnumerable<IInvoice>> GetAllInvoicesAsync();
        Task<IEnumerable<IInvoice>> GetPaginatedInvoicesAsync(InvoicePaginationQuery query);
        Task<IInvoice> GetInvoiceByIdAsync(long invoiceId);
        Task<long> GetNumberOfInvoicesAsync();
        Task<IInvoice> CreateInvoiceAsync(InvoiceCreateRequest request);
        Task<IInvoice> UpdateInvoiceAsync(InvoiceUpdateRequest request, long invoiceId);
        Task<IInvoice> PatchInvoiceAsync(JsonPatchDocument<InvoicePatchRequest> request, long invoiceId);
        Task DeleteInvoiceAsync(long invoiceId);
    }
}
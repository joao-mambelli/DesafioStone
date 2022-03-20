using DesafioStone.DataContracts;
using DesafioStone.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioStone.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<IEnumerable<Invoice>> GetPaginatedInvoicesAsync(InvoicePaginationQuery query);
        Task<Invoice> GetInvoiceByIdAsync(long invoiceId);
        Task<long> GetNumberOfInvoicesAsync();
        Task<Invoice> CreateInvoiceAsync(InvoiceCreateRequest request);
        Task<Invoice> UpdateInvoiceAsync(InvoiceUpdateRequest request, long invoiceId);
        Task<Invoice> PatchInvoiceAsync(JsonPatchDocument<InvoicePatchRequest> request, long invoiceId);
        Task DeleteInvoiceAsync(long invoiceId);
    }
}
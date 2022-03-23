using DesafioStone.DataContracts;
using DesafioStone.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioStone.Interfaces.Services
{
    public interface IInvoiceService
    {
        IEnumerable<Invoice> GetInvoices(InvoiceQuery query);
        Invoice GetInvoiceById(long invoiceId);
        long GetNumberOfInvoices();
        Invoice CreateInvoice(InvoiceCreateRequest request);
        Invoice UpdateInvoice(InvoiceUpdateRequest request, long invoiceId);
        Invoice PatchInvoice(JsonPatchDocument<InvoicePatchRequest> request, long invoiceId);
        void DeleteInvoice(long invoiceId);
    }
}
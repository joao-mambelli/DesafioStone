using DesafioStone.Interfaces.Repositories;
using DesafioStone.Interfaces.Services;
using DesafioStone.Models;
using DesafioStone.DataContracts;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using System.Net;
using Microsoft.AspNetCore.JsonPatch;

namespace DesafioStone.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;

        public InvoiceService(IInvoiceRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Invoice> GetInvoices(InvoiceQuery query)
        {
            return _repository.GetInvoices(query);
        }

        public Invoice GetInvoiceById(long invoiceId)
        {
            var invoice = _repository.GetInvoiceById(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            return invoice;
        }

        public long GetNumberOfInvoices()
        {
            return _repository.GetInvoiceCount();
        }

        public Invoice CreateInvoice(InvoiceCreateRequest request)
        {
            var insertedId = _repository.InsertInvoice(new Invoice(request));

            return _repository.GetInvoiceById(insertedId);
        }

        public Invoice UpdateInvoice(InvoiceUpdateRequest request, long invoiceId)
        {
            var invoice = _repository.GetInvoiceById(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            invoice.ReferenceMonth = request.ReferenceMonth ?? MonthEnum.January;
            invoice.ReferenceYear = request.ReferenceYear ?? 0;
            invoice.Document = request.Document;
            invoice.Amount = request.Amount ?? 0;
            invoice.Description = request.Description;

            _repository.UpdateInvoice(invoice);

            return invoice;
        }

        public Invoice PatchInvoice(JsonPatchDocument<InvoicePatchRequest> request, long invoiceId)
        {
            var invoice = _repository.GetInvoiceById(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            var invoicePatch = new InvoicePatchRequest(invoice);

            request.ApplyTo(invoicePatch);

            invoice.ReferenceMonth = invoicePatch.ReferenceMonth;
            invoice.ReferenceYear = invoicePatch.ReferenceYear;
            invoice.Document = invoicePatch.Document;
            invoice.Amount = invoicePatch.Amount;
            invoice.Description = invoicePatch.Description;

            _repository.UpdateInvoice(invoice);

            return invoice;
        }

        public void DeleteInvoice(long invoiceId)
        {
            var invoice = _repository.GetInvoiceById(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            _repository.DeleteInvoice(invoiceId);
        }
    }
}

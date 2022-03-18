﻿using DesafioStone.Interfaces.ModelsInterfaces;
using DesafioStone.Interfaces.RepositoriesInterfaces;
using DesafioStone.Interfaces.ServicesInterfaces;
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

        public async Task<IEnumerable<IInvoice>> GetAllInvoicesAsync()
        {
            var invoices = await _repository.GetAllInvoicesAsync();

            return invoices;
        }

        public async Task<IEnumerable<IInvoice>> GetPaginatedInvoicesAsync(InvoicePaginationQuery query)
        {
            return await _repository.GetInvoicesOffsetAsync(query.Page - 1, query.RowsPerPage);
        }

        public async Task<IInvoice> GetInvoiceByIdAsync(long invoiceId)
        {
            var invoice = await _repository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            return invoice;
        }

        public async Task<long> GetNumberOfInvoicesAsync()
        {
            return await _repository.GetInvoiceCountAsync();
        }

        public async Task<IInvoice> CreateInvoiceAsync(InvoiceCreateRequest request)
        {
            var insertedId = await _repository.InsertInvoiceAsync(new Invoice()
            {
                ReferenceMonth = request.ReferenceMonth ?? MonthEnum.January,
                ReferenceYear = request.ReferenceYear ?? 0,
                Amount = request.Amount ?? 0,
                Description = request.Description,
                Document = request.Document
            });

            return await _repository.GetInvoiceByIdAsync(insertedId);
        }

        public async Task<IInvoice> UpdateInvoiceAsync(InvoiceUpdateRequest request, long invoiceId)
        {
            var invoice = await _repository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            invoice.ReferenceMonth = request.ReferenceMonth ?? MonthEnum.January;
            invoice.ReferenceYear = request.ReferenceYear ?? 0;
            invoice.Document = request.Document;
            invoice.Amount = request.Amount ?? 0;
            invoice.Description = request.Description;

            await _repository.UpdateInvoiceAsync(invoice);

            return invoice;
        }

        public async Task<IInvoice> PatchInvoiceAsync(JsonPatchDocument<InvoicePatchRequest> request, long invoiceId)
        {
            var invoice = await _repository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            var invoicePatch = Helpers.InvoiceToInvoicePatchRequest(invoice);

            request.ApplyTo(invoicePatch);

            invoice.ReferenceMonth = invoicePatch.ReferenceMonth;
            invoice.ReferenceYear = invoicePatch.ReferenceYear;
            invoice.Document = invoicePatch.Document;
            invoice.Amount = invoicePatch.Amount;
            invoice.Description = invoicePatch.Description;

            await _repository.UpdateInvoiceAsync(invoice);

            return invoice;
        }

        public async Task DeleteInvoiceAsync(long invoiceId)
        {
            var invoice = await _repository.GetInvoiceByIdAsync(invoiceId);

            if (invoice == null)
                throw Helpers.BuildHttpException(HttpStatusCode.NotFound, "Invoice not found.");

            await _repository.DeleteInvoiceAsync(invoiceId);
        }
    }
}

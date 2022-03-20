using DesafioStone.Models;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using MySql.Data.MySqlClient;
using DesafioStone.Interfaces.Repositories;

namespace DesafioStone.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync(bool active = true)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE isactive = @isActive", conn);
            cmd.Parameters.AddWithValue("isActive", active ? 1 : 0);

            var invoices = new List<Invoice>();

            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                invoices.Add(new Invoice
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    ReferenceMonth = Helpers.ConvertFromDBVal<MonthEnum>(rdr["month"]),
                    ReferenceYear = Helpers.ConvertFromDBVal<int>(rdr["year"]),
                    Document = Helpers.ConvertFromDBVal<string>(rdr["document"]),
                    Description = Helpers.ConvertFromDBVal<string>(rdr["description"]),
                    Amount = Helpers.ConvertFromDBVal<int>(rdr["amount"]),
                    IsActive = Helpers.ConvertFromDBVal<bool>(rdr["isactive"]),
                    CreatedAt = Helpers.ConvertFromDBVal<DateTime>(rdr["createdat"]),
                    DeactivatedAt = Helpers.ConvertFromDBVal<DateTime?>(rdr["deactivatedat"])
                });
            }

            return invoices;
        }

        public async Task<IEnumerable<Invoice>> GetInvoicesOffsetAsync(int offset, int amount, bool active = true)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE isactive = @isActive LIMIT @offset, @amount", conn);
            cmd.Parameters.AddWithValue("offset", offset);
            cmd.Parameters.AddWithValue("amount", amount);
            cmd.Parameters.AddWithValue("isactive", active ? 1 : 0);

            var invoices = new List<Invoice>();

            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                invoices.Add(new Invoice
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    ReferenceMonth = Helpers.ConvertFromDBVal<MonthEnum>(rdr["month"]),
                    ReferenceYear = Helpers.ConvertFromDBVal<int>(rdr["year"]),
                    Document = Helpers.ConvertFromDBVal<string>(rdr["document"]),
                    Description = Helpers.ConvertFromDBVal<string>(rdr["description"]),
                    Amount = Helpers.ConvertFromDBVal<int>(rdr["amount"]),
                    IsActive = Helpers.ConvertFromDBVal<bool>(rdr["isactive"]),
                    CreatedAt = Helpers.ConvertFromDBVal<DateTime>(rdr["createdat"]),
                    DeactivatedAt = Helpers.ConvertFromDBVal<DateTime?>(rdr["deactivatedat"])
                });
            }

            return invoices;
        }

        public async Task<Invoice> GetInvoiceByIdAsync(long invoiceId, bool active = true)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE id = @invoiceId AND isActive = @isActive", conn);
            cmd.Parameters.AddWithValue("invoiceId", invoiceId);
            cmd.Parameters.AddWithValue("isActive", active ? 1 : 0);

            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new Invoice
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    ReferenceMonth = Helpers.ConvertFromDBVal<MonthEnum>(rdr["month"]),
                    ReferenceYear = Helpers.ConvertFromDBVal<int>(rdr["year"]),
                    Document = Helpers.ConvertFromDBVal<string>(rdr["document"]),
                    Description = Helpers.ConvertFromDBVal<string>(rdr["description"]),
                    Amount = Helpers.ConvertFromDBVal<int>(rdr["amount"]),
                    IsActive = Helpers.ConvertFromDBVal<bool>(rdr["isactive"]),
                    CreatedAt = Helpers.ConvertFromDBVal<DateTime>(rdr["createdat"]),
                    DeactivatedAt = Helpers.ConvertFromDBVal<DateTime?>(rdr["deactivatedat"])
                };
            }

            return null;
        }

        public async Task<long> GetInvoiceCountAsync(bool active = true)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("SELECT COUNT(*) FROM invoice WHERE isactive = @isactive", conn);
            cmd.Parameters.AddWithValue("isactive", active ? 1 : 0);

            using var rdr = await cmd.ExecuteReaderAsync();
            await rdr.ReadAsync();

            return Helpers.ConvertFromDBVal<long>(rdr["COUNT(*)"]);
        }

        public async Task<long> InsertInvoiceAsync(Invoice invoice)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("INSERT INTO invoice (month, year, document, description, amount, createdat) VALUES (@month, @year, @document, @description, @amount, @createdAt)", conn);
            cmd.Parameters.AddWithValue("month", (int)invoice.ReferenceMonth);
            cmd.Parameters.AddWithValue("year", invoice.ReferenceYear);
            cmd.Parameters.AddWithValue("document", invoice.Document);
            cmd.Parameters.AddWithValue("description", invoice.Description);
            cmd.Parameters.AddWithValue("amount", invoice.Amount);
            cmd.Parameters.AddWithValue("createdAt", DateTime.UtcNow);

            await cmd.ExecuteNonQueryAsync();

            return cmd.LastInsertedId;
        }

        public async Task UpdateInvoiceAsync(Invoice invoice)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("UPDATE invoice SET month = @month, year = @year, document = @document, description = @description, amount = @amount WHERE id = @invoiceId", conn);
            cmd.Parameters.AddWithValue("month", invoice.ReferenceMonth);
            cmd.Parameters.AddWithValue("year", invoice.ReferenceYear);
            cmd.Parameters.AddWithValue("document", invoice.Document);
            cmd.Parameters.AddWithValue("description", invoice.Description);
            cmd.Parameters.AddWithValue("amount", invoice.Amount);
            cmd.Parameters.AddWithValue("invoiceId", invoice.Id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteInvoiceAsync(long invoiceId)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("UPDATE invoice SET isActive = @isActive, deactivatedat = @deactivatedAt WHERE id = @invoiceId", conn);
            cmd.Parameters.AddWithValue("isActive", 0);
            cmd.Parameters.AddWithValue("deactivatedat", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("invoiceId", invoiceId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}

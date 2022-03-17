using DesafioStone.Models;
using DesafioStone.Interfaces;
using DesafioStone.DataContracts;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using MySql.Data.MySqlClient;

namespace DesafioStone.Repositories
{
    public static class InvoiceRepository
    {
        public static async Task<IInvoice> CreateInvoiceAsync(InvoiceCreateRequest request)
        {
            try
            {
                using var conn = new MySqlConnection(DBAccess.ConnectionString());
                conn.Open();

                using var cmd = new MySqlCommand("INSERT INTO invoice (month, year, document, description, amount, createdat) VALUES (@month, @year, @document, @description, @amount, @createdAt)", conn);
                cmd.Parameters.AddWithValue("month", (int)request.ReferenceMonth);
                cmd.Parameters.AddWithValue("year", request.ReferenceYear);
                cmd.Parameters.AddWithValue("document", request.Document);
                cmd.Parameters.AddWithValue("description", request.Description);
                cmd.Parameters.AddWithValue("amount", request.Amount);
                cmd.Parameters.AddWithValue("createdAt", DateTime.UtcNow);

                await cmd.ExecuteNonQueryAsync();

                return await GetInvoiceByIdAsync(cmd.LastInsertedId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<IInvoice> GetInvoiceByIdAsync(long invoiceId)
        {
            try
            {
                IInvoice invoice;

                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE id = @invoiceId", conn);
                    cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                    using var rdr = await cmd.ExecuteReaderAsync();
                    await rdr.ReadAsync();

                    invoice = new Invoice
                    {
                        Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                        ReferenceMonth = Helpers.ConvertFromDBVal<Month>(rdr["month"]),
                        ReferenceYear = Helpers.ConvertFromDBVal<int>(rdr["year"]),
                        Document = Helpers.ConvertFromDBVal<string>(rdr["document"]),
                        Description = Helpers.ConvertFromDBVal<string>(rdr["description"]),
                        Amount = Helpers.ConvertFromDBVal<int>(rdr["amount"]),
                        IsActive = Helpers.ConvertFromDBVal<bool>(rdr["isactive"]),
                        CreatedAt = Helpers.ConvertFromDBVal<DateTime>(rdr["createdat"]),
                        DeactivatedAt = Helpers.ConvertFromDBVal<DateTime?>(rdr["deactivatedat"])
                    };
                }

                return invoice;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<IEnumerable<IInvoice>> GetAllActiveInvoicesAsync()
        {
            try
            {
                IList<IInvoice> invoices = new List<IInvoice>();

                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE isactive = @isActive", conn);
                    cmd.Parameters.AddWithValue("isActive", 1);

                    using var rdr = await cmd.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                    {
                        invoices.Add(new Invoice
                        {
                            Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                            ReferenceMonth = Helpers.ConvertFromDBVal<Month>(rdr["month"]),
                            ReferenceYear = Helpers.ConvertFromDBVal<int>(rdr["year"]),
                            Document = Helpers.ConvertFromDBVal<string>(rdr["document"]),
                            Description = Helpers.ConvertFromDBVal<string>(rdr["description"]),
                            Amount = Helpers.ConvertFromDBVal<int>(rdr["amount"]),
                            IsActive = Helpers.ConvertFromDBVal<bool>(rdr["isactive"]),
                            CreatedAt = Helpers.ConvertFromDBVal<DateTime>(rdr["createdat"]),
                            DeactivatedAt = Helpers.ConvertFromDBVal<DateTime?>(rdr["deactivatedat"])
                        });
                    }
                }

                return invoices;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<IEnumerable<IInvoice>> GetActivePaginatedInvoicesAsync(InvoicePaginationQuery query)
        {
            try
            {
                IList<IInvoice> invoices = new List<IInvoice>();

                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE isactive = @isActive LIMIT @offset, @rows", conn);
                    cmd.Parameters.AddWithValue("offset", (query.Page - 1) * query.RowsPerPage);
                    cmd.Parameters.AddWithValue("rows", query.RowsPerPage);
                    cmd.Parameters.AddWithValue("isactive", 1);

                    using var rdr = await cmd.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                    {
                        invoices.Add(new Invoice
                        {
                            Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                            ReferenceMonth = Helpers.ConvertFromDBVal<Month>(rdr["month"]),
                            ReferenceYear = Helpers.ConvertFromDBVal<int>(rdr["year"]),
                            Document = Helpers.ConvertFromDBVal<string>(rdr["document"]),
                            Description = Helpers.ConvertFromDBVal<string>(rdr["description"]),
                            Amount = Helpers.ConvertFromDBVal<int>(rdr["amount"]),
                            IsActive = Helpers.ConvertFromDBVal<bool>(rdr["isactive"]),
                            CreatedAt = Helpers.ConvertFromDBVal<DateTime>(rdr["createdat"]),
                            DeactivatedAt = Helpers.ConvertFromDBVal<DateTime?>(rdr["deactivatedat"])
                        });
                    }
                }

                return invoices;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<long?> GetNumberOfActiveInvoicesAsync()
        {
            try
            {
                long count;

                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("SELECT COUNT(*) FROM invoice WHERE isactive = @isactive", conn);
                    cmd.Parameters.AddWithValue("isactive", 1);

                    using var rdr = await cmd.ExecuteReaderAsync();
                    await rdr.ReadAsync();

                    count = Helpers.ConvertFromDBVal<long>(rdr["COUNT(*)"]);
                }

                return count;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task DeleteInvoiceAsync(long invoiceId)
        {
            try
            {
                using var conn = new MySqlConnection(DBAccess.ConnectionString());
                conn.Open();

                using var cmd = new MySqlCommand("UPDATE invoice SET isActive = @isActive, deactivatedat = @deactivatedAt WHERE id = @invoiceId", conn);
                cmd.Parameters.AddWithValue("isActive", 0);
                cmd.Parameters.AddWithValue("deactivatedat", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception)
            {
                
            }
        }

        public static async Task<IInvoice> UpdateInvoiceAsync(InvoiceUpdateRequest request, long invoiceId)
        {
            try
            {
                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("UPDATE invoice SET month = @month, year = @year, document = @document, description = @description, amount = @amount WHERE id = @invoiceId", conn);
                    cmd.Parameters.AddWithValue("month", request.ReferenceMonth);
                    cmd.Parameters.AddWithValue("year", request.ReferenceYear);
                    cmd.Parameters.AddWithValue("document", request.Document);
                    cmd.Parameters.AddWithValue("description", request.Description);
                    cmd.Parameters.AddWithValue("amount", request.Amount);
                    cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                    await cmd.ExecuteNonQueryAsync();
                }

                return await GetInvoiceByIdAsync(invoiceId);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<IInvoice> PatchInvoiceAsync(InvoicePatchRequest request, long invoiceId)
        {
            try
            {
                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("UPDATE invoice SET month = @month, year = @year, document = @document, description = @description, amount = @amount WHERE id = @invoiceId", conn);
                    cmd.Parameters.AddWithValue("month", request.ReferenceMonth);
                    cmd.Parameters.AddWithValue("year", request.ReferenceYear);
                    cmd.Parameters.AddWithValue("document", request.Document);
                    cmd.Parameters.AddWithValue("description", request.Description);
                    cmd.Parameters.AddWithValue("amount", request.Amount);
                    cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                    await cmd.ExecuteNonQueryAsync();
                }

                return await GetInvoiceByIdAsync(invoiceId);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

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
        public static IInvoice CreateInvoice(InvoiceCreateRequest request)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("INSERT INTO invoice (month, year, document, description, amount, createdat) VALUES (@month, @year, @document, @description, @amount, @createdAt)", conn);

                cmd.Parameters.AddWithValue("month", (int)request.ReferenceMonth);
                cmd.Parameters.AddWithValue("year", request.ReferenceYear);
                cmd.Parameters.AddWithValue("document", request.Document);
                cmd.Parameters.AddWithValue("description", request.Description);
                cmd.Parameters.AddWithValue("amount", request.Amount);
                cmd.Parameters.AddWithValue("createdAt", DateTime.UtcNow);

                cmd.ExecuteNonQuery();

                return GetInvoiceById(cmd.LastInsertedId);
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static IInvoice GetInvoiceById(long invoiceId)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE id = @invoiceId", conn);

                cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                var rdr = cmd.ExecuteReader();
                rdr.Read();

                var invoice = new Invoice
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

                rdr.Close();

                return invoice;
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static IEnumerable<IInvoice> GetAllInvoices()
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice", conn);

                var rdr = cmd.ExecuteReader();

                var invoices = new List<IInvoice>();

                while(rdr.Read())
                {
                    var invoice = new Invoice
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

                    invoices.Add(invoice);
                }

                rdr.Close();

                return invoices;
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static void DeleteInvoice(long invoiceId)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("UPDATE invoice SET isActive = @isActive, deactivatedat = @deactivatedAt WHERE id = @invoiceId", conn);

                cmd.Parameters.AddWithValue("isActive", 0);
                cmd.Parameters.AddWithValue("deactivatedat", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                conn.Close();
            }
        }

        public static IInvoice UpdateInvoice(InvoiceUpdateRequest request, long invoiceId)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("UPDATE invoice SET month = @month, year = @year, document = @document, description = @description, amount = @amount WHERE id = @invoiceId", conn);

                cmd.Parameters.AddWithValue("month", request.ReferenceMonth);
                cmd.Parameters.AddWithValue("year", request.ReferenceYear);
                cmd.Parameters.AddWithValue("document", request.Document);
                cmd.Parameters.AddWithValue("description", request.Description);
                cmd.Parameters.AddWithValue("amount", request.Amount);
                cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                cmd.ExecuteNonQuery();

                return GetInvoiceById(invoiceId);
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static IInvoice PatchInvoice(InvoicePatchRequest request, long invoiceId)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("UPDATE invoice SET month = @month, year = @year, document = @document, description = @description, amount = @amount WHERE id = @invoiceId", conn);

                cmd.Parameters.AddWithValue("month", request.ReferenceMonth);
                cmd.Parameters.AddWithValue("year", request.ReferenceYear);
                cmd.Parameters.AddWithValue("document", request.Document);
                cmd.Parameters.AddWithValue("description", request.Description);
                cmd.Parameters.AddWithValue("amount", request.Amount);
                cmd.Parameters.AddWithValue("invoiceId", invoiceId);

                cmd.ExecuteNonQuery();

                return GetInvoiceById(invoiceId);
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }
    }
}

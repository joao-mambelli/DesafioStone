using DesafioStone.Models;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using MySql.Data.MySqlClient;
using DesafioStone.Interfaces.Repositories;
using DesafioStone.DataContracts;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using DesafioStone.Interfaces.Factories;

namespace DesafioStone.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public InvoiceRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<Invoice> GetInvoices(InvoiceQuery query, bool active = true)
        {
            var cmdText = "SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE isactive = @isActive ";

            if (query.Document != null)
                cmdText += "AND document = @document ";

            if (query.Year != null)
                cmdText += "AND year = @year ";

            if (query.Month != null)
                cmdText += "AND month = @month ";

            if (query.Orders != null)
            {
                cmdText += "ORDER BY ";

                var list = query.Orders.DistinctBy(e => e.Field);
                foreach (var order in list)
                {
                    cmdText += JsonConvert.SerializeObject(order.Field, new StringEnumConverter()).Replace("\"", "");

                    if (order.Direction == OrderDirectionEnum.Descending)
                        cmdText += " DESC";

                    if (order != list.Last())
                        cmdText += ", ";
                    else
                        cmdText += " ";
                }
            }

            cmdText += "LIMIT @offset, @amount";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("offset", (query.Page - 1) * query.RowsPerPage);
                        command.AddWithValue("amount", query.RowsPerPage);
                        command.AddWithValue("isactive", active ? 1 : 0);

                        if (query.Document != null)
                            command.AddWithValue("document", query.Document);

                        if (query.Year != null)
                            command.AddWithValue("year", query.Year);

                        if (query.Month != null)
                            command.AddWithValue("month", query.Month);
                    }

                    connection.Open();

                    if (!isMySql)
                    {
                        return null;
                    }

                    using (var rdr = command.ExecuteReader())
                    {
                        var invoices = new List<Invoice>();

                        while (rdr.Read())
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
                }
            }
        }

        public Invoice GetInvoiceById(long invoiceId, bool active = true)
        {
            var cmdText = "SELECT id, month, year, document, description, amount, isactive, createdat, deactivatedat FROM invoice WHERE id = @invoiceId AND isActive = @isActive";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("invoiceId", invoiceId);
                        command.AddWithValue("isActive", active ? 1 : 0);
                    }

                    connection.Open();

                    if (!isMySql)
                    {
                        return null;
                    }

                    using (var rdr = command.ExecuteReader())
                    {
                        if (rdr.Read())
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
                }
            }
        }

        public long GetInvoiceCount(bool active = true)
        {
            var cmdText = "SELECT COUNT(*) FROM invoice WHERE isactive = @isactive";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("isactive", active ? 1 : 0);
                    }

                    connection.Open();

                    if (!isMySql)
                    {
                        return 0;
                    }

                    using (var rdr = command.ExecuteReader())
                    {
                        rdr.Read();

                        return Helpers.ConvertFromDBVal<long>(rdr["COUNT(*)"]);
                    }
                }
            }
        }

        public long InsertInvoice(Invoice invoice)
        {
            var cmdText = "INSERT INTO invoice (month, year, document, description, amount, createdat) VALUES (@month, @year, @document, @description, @amount, @createdAt)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("month", (int)invoice.ReferenceMonth);
                        command.AddWithValue("year", invoice.ReferenceYear);
                        command.AddWithValue("document", invoice.Document);
                        command.AddWithValue("description", invoice.Description);
                        command.AddWithValue("amount", invoice.Amount);
                        command.AddWithValue("createdAt", DateTime.UtcNow);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    if (!isMySql)
                    {
                        return 0;
                    }

                    return ((MySqlCommand)command).LastInsertedId;
                }
            }
        }

        public void UpdateInvoice(Invoice invoice)
        {
            var cmdText = "UPDATE invoice SET month = @month, year = @year, document = @document, description = @description, amount = @amount WHERE id = @invoiceId";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("month", invoice.ReferenceMonth);
                        command.AddWithValue("year", invoice.ReferenceYear);
                        command.AddWithValue("document", invoice.Document);
                        command.AddWithValue("description", invoice.Description);
                        command.AddWithValue("amount", invoice.Amount);
                        command.AddWithValue("invoiceId", invoice.Id);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteInvoice(long invoiceId)
        {
            var cmdText = "UPDATE invoice SET isActive = @isActive, deactivatedat = @deactivatedAt WHERE id = @invoiceId";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("isActive", 0);
                        command.AddWithValue("deactivatedat", DateTime.UtcNow);
                        command.AddWithValue("invoiceId", invoiceId);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

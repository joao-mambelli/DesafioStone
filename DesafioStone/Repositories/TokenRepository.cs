using DesafioStone.Interfaces.Repositories;
using DesafioStone.Utils.Common;
using DesafioStone.Interfaces.Factories;
using MySql.Data.MySqlClient;

namespace DesafioStone.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TokenRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public string GetRefreshTokenByUserId(long userId)
        {
            var cmdText = "SELECT refreshtoken FROM refreshToken WHERE userid = @userId";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("userId", userId);
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
                            return Helpers.ConvertFromDBVal<string>(rdr["refreshtoken"]);
                        }

                        return null;
                    }
                }
            }
        }

        public void InsertRefreshToken(long userId, string refreshToken)
        {
            var cmdText = "INSERT INTO refreshToken (userid, refreshtoken) VALUES (@userId, @refreshToken)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("userId", userId);
                        command.AddWithValue("refreshToken", refreshToken);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();

                    return;
                }
            }
        }

        public void DeleteRefreshToken(long userId)
        {
            var cmdText = "DELETE FROM refreshToken WHERE userid = @userId";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("userId", userId);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

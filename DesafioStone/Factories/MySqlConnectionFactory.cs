using DesafioStone.Interfaces.Factories;
using DesafioStone.Interfaces.Providers;
using MySql.Data.MySqlClient;
using System.Data;

namespace DesafioStone.Factories
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        private readonly IConnectionStringProvider _accessDataBaseProvider;

        public MySqlConnectionFactory(IConnectionStringProvider accessDataBaseProvider)
        {
            _accessDataBaseProvider = accessDataBaseProvider;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_accessDataBaseProvider.ConnectionString());
        }
    }
}

using DesafioStone.Interfaces.Factories;
using DesafioStone.Utils.Common;
using MySql.Data.MySqlClient;
using System.Data;

namespace DesafioStone.Factories
{
    public class MySqlConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(AccessDataBase.ConnectionString());
        }
    }
}

using MySql.Data.MySqlClient;
using System.Data;

namespace DesafioStone.Interfaces.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

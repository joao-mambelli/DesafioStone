using DesafioStone.Interfaces.Repositories;
using DesafioStone.Models;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using DesafioStone.Interfaces.Factories;
using MySql.Data.MySqlClient;

namespace DesafioStone.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public UserRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<User> GetAllUsers(bool active = true)
        {
            var cmdText = "SELECT id, username, password, role, lastlogoutallrequest FROM user WHERE isActive = @isActive";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("isActive", active ? 1 : 0);
                    }

                    connection.Open();

                    if (!isMySql)
                    {
                        return null;
                    }

                    using (var rdr = command.ExecuteReader())
                    {
                        var list = new List<User>();

                        while (rdr.Read())
                        {
                            list.Add(new User
                            {
                                Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                                Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                                Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                                Role = Helpers.ConvertFromDBVal<RoleEnum>(rdr["role"]),
                                LastLogoutAllRequest = Helpers.ConvertFromDBVal<DateTime?>(rdr["lastlogoutallrequest"])
                            });
                        }

                        return list;
                    }
                }
            }
        }

        public User GetUserById(long userId, bool active = true)
        {
            var cmdText = "SELECT id, username, password, role, lastlogoutallrequest FROM user WHERE id = @userId AND isActive = @isActive";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("userId", userId);
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
                            return new User
                            {
                                Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                                Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                                Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                                Role = Helpers.ConvertFromDBVal<RoleEnum>(rdr["role"]),
                                LastLogoutAllRequest = Helpers.ConvertFromDBVal<DateTime?>(rdr["lastlogoutallrequest"])
                            };
                        }

                        return null;
                    }
                }
            }
        }

        public User GetUserByUsername(string username, bool active = true)
        {
            var cmdText = "SELECT id, username, password, role, lastlogoutallrequest FROM user WHERE username = @username AND isActive = @isActive";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("username", username);
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
                            return new User
                            {
                                Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                                Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                                Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                                Role = Helpers.ConvertFromDBVal<RoleEnum>(rdr["role"]),
                                LastLogoutAllRequest = Helpers.ConvertFromDBVal<DateTime?>(rdr["lastlogoutallrequest"])
                            };
                        }

                        return null;
                    }
                }
            }
        }

        public long InsertUser(User user)
        {
            var cmdText = "INSERT INTO user (username, password, role) VALUES (@username, @password, @role)";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("username", user.Username);
                        command.AddWithValue("password", user.Password);
                        command.AddWithValue("role", (int)user.Role);
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

        public void UpdateUser(User user)
        {
            var cmdText = "UPDATE user SET username = @username, password = @password, lastlogoutallrequest = @lastlogoutallrequest WHERE id = @userId";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("username", user.Username);
                        command.AddWithValue("password", user.Password);
                        command.AddWithValue("lastlogoutallrequest", user.LastLogoutAllRequest);
                        command.AddWithValue("userId", user.Id);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(long userId)
        {
            var cmdText = "UPDATE user SET isActive = @isActive WHERE id = @userId";

            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    bool isMySql = command.GetType().Equals(typeof(MySqlCommand));
                    command.CommandText = cmdText;

                    if (isMySql)
                    {
                        command.AddWithValue("isActive", 0);
                        command.AddWithValue("userId", userId);
                    }

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}

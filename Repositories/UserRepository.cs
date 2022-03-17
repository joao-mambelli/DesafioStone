using DesafioStone.Interfaces;
using DesafioStone.Models;
using DesafioStone.DataContracts;
using DesafioStone.Enums;
using DesafioStone.Utils.Providers.HashProvider;
using DesafioStone.Utils.Common;
using MySql.Data.MySqlClient;

namespace DesafioStone.Repositories
{
    public static class UserRepository
    {
        public static IUser VerifyPassword(string username, string password)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE isactive = @isActive AND username = @username", conn);

                cmd.Parameters.AddWithValue("isActive", 1);
                cmd.Parameters.AddWithValue("username", username);

                var rdr = cmd.ExecuteReader();

                var users = new List<IUser>();

                while (rdr.Read())
                {
                    users.Add(new User
                    {
                        Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                        Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                        Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                        Role = Helpers.ConvertFromDBVal<Role>(rdr["role"])
                    });
                }

                rdr.Close();

                var user = users.Where(x => x.Username == username.ToLower()).FirstOrDefault();

                if (user != null && Password.IsValid(password, user.Password))
                {
                    user.Password = "";

                    return user;
                }

                return null;
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static IUser CreateUser(UserCreateRequest request)
        {
            IHashProvider hashProvider = new HashProvider();

            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("INSERT INTO user (username, password, role) VALUES (@username, @password, @role)", conn);

                cmd.Parameters.AddWithValue("username", request.Username);
                cmd.Parameters.AddWithValue("password", hashProvider.ComputeHash(request.Password));
                cmd.Parameters.AddWithValue("role", (int)request.Role);

                cmd.ExecuteNonQuery();

                return GetUserById(cmd.LastInsertedId);
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static IUser GetUserByUsername(string username)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE isactive = @isActive AND username = @username", conn);

                cmd.Parameters.AddWithValue("isActive", 1);
                cmd.Parameters.AddWithValue("username", username);

                var rdr = cmd.ExecuteReader();
                rdr.Read();

                var user = new User
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                    Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                    Role = Helpers.ConvertFromDBVal<Role>(rdr["role"])
                };

                rdr.Close();

                return user;
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static IUser GetUserById(long userId)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE id = @userId", conn);

                cmd.Parameters.AddWithValue("userId", userId);

                var rdr = cmd.ExecuteReader();
                rdr.Read();

                var user = new User
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                    Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                    Role = Helpers.ConvertFromDBVal<Role>(rdr["role"])
                };

                rdr.Close();

                return user;
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static void DeleteUser(long userId)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("UPDATE user SET isActive = @isActive WHERE id = @userId", conn);

                cmd.Parameters.AddWithValue("isActive", 0);
                cmd.Parameters.AddWithValue("userId", userId);

                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                conn.Close();
            }
        }

        public static IUser UpdateUserPassword(UserUpdatePasswordRequest request, long userId)
        {
            IHashProvider hashProvider = new HashProvider();

            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("UPDATE user SET password = @password WHERE id = @userId", conn);

                cmd.Parameters.AddWithValue("password", hashProvider.ComputeHash(request.Password));
                cmd.Parameters.AddWithValue("userId", userId);

                cmd.ExecuteNonQuery();

                return GetUserById(userId);
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }
    }
}

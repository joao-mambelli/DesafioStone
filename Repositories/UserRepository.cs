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
        public static User VerifyPassword(string username, string password)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE isactive = @isActive AND username = @username", conn);

                cmd.Parameters.AddWithValue("isActive", 1);
                cmd.Parameters.AddWithValue("username", username);

                var rdr = cmd.ExecuteReader();

                var users = new List<User>();

                while (rdr.Read())
                {
                    users.Add(new User
                    {
                        Id = (int)rdr["id"],
                        Username = (string)rdr["username"],
                        Password = (string)rdr["password"],
                        Role = (Role)rdr["role"]
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

        public static User CreateUser(UserCreateRequest request)
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

                return GetUserByUsername(request.Username);
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static User GetUserByUsername(string username)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE isactive = @isActive AND username = @username", conn);

                cmd.Parameters.AddWithValue("isActive", 1);
                cmd.Parameters.AddWithValue("username", username);

                var rdr = cmd.ExecuteReader();

                var users = new List<User>();

                while (rdr.Read())
                {
                    users.Add(new User
                    {
                        Id = (int)rdr["id"],
                        Username = (string)rdr["username"],
                        Password = (string)rdr["password"],
                        Role = (Role)rdr["role"]
                    });
                }

                rdr.Close();

                return users.Where(x => x.Username == username.ToLower()).FirstOrDefault();
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static User GetUserById(int userId)
        {
            var conn = new MySqlConnection(DBAccess.ConnectionString());

            try
            {
                conn.Open();

                var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE id = @userId", conn);

                cmd.Parameters.AddWithValue("userId", userId);

                var rdr = cmd.ExecuteReader();

                var users = new List<User>();

                while (rdr.Read())
                {
                    users.Add(new User
                    {
                        Id = (int)rdr["id"],
                        Username = (string)rdr["username"],
                        Password = (string)rdr["password"],
                        Role = (Role)rdr["role"]
                    });
                }

                rdr.Close();

                return users.Where(x => x.Id == userId).FirstOrDefault();
            }
            catch (Exception)
            {
                conn.Close();

                return null;
            }
        }

        public static void DeleteUser(int userId)
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
    }
}

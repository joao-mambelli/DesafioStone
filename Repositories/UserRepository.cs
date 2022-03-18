using DesafioStone.Interfaces;
using DesafioStone.Models;
using DesafioStone.DataContracts;
using DesafioStone.Enums;
using DesafioStone.Utils.Providers.HashProvider;
using DesafioStone.Utils.Common;
using MySql.Data.MySqlClient;
using DesafioStone.Entities;

namespace DesafioStone.Repositories
{
    public static class UserRepository
    {
        public static async Task<IObjectException<IUser>> VerifyPasswordAsync(string username, string password)
        {
            try
            {
                var users = new List<IUser>();

                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE isactive = @isActive AND username = @username", conn);
                    cmd.Parameters.AddWithValue("isActive", 1);
                    cmd.Parameters.AddWithValue("username", username);

                    using var rdr = await cmd.ExecuteReaderAsync();
                    while (await rdr.ReadAsync())
                    {
                        users.Add(new User
                        {
                            Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                            Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                            Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                            Role = Helpers.ConvertFromDBVal<Role>(rdr["role"])
                        });
                    }
                }

                var user = users.Where(x => x.Username == username.ToLower()).FirstOrDefault();

                if (user != null && Password.IsValid(password, user.Password))
                {
                    user.Password = "";

                    return new ObjectException<IUser>(user);
                }

                return new ObjectException<IUser>(null, null);
            }
            catch (Exception ex)
            {
                return new ObjectException<IUser>(ex);
            }
        }

        public static async Task<IObjectException<IUser>> CreateUserAsync(UserCreateRequest request)
        {
            IHashProvider hashProvider = new HashProvider();

            try
            {
                using var conn = new MySqlConnection(DBAccess.ConnectionString());
                conn.Open();

                using var cmd = new MySqlCommand("INSERT INTO user (username, password, role) VALUES (@username, @password, @role)", conn);
                cmd.Parameters.AddWithValue("username", request.Username);
                cmd.Parameters.AddWithValue("password", hashProvider.ComputeHash(request.Password));
                cmd.Parameters.AddWithValue("role", (int)request.Role);

                await cmd.ExecuteNonQueryAsync();

                return await GetUserByIdAsync(cmd.LastInsertedId);
            }
            catch (Exception ex)
            {
                return new ObjectException<IUser>(ex);
            }
        }

        public static async Task<IObjectException<IUser>> GetUserByUsernameAsync(string username)
        {
            try
            {
                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE isactive = @isActive AND username = @username", conn);
                    cmd.Parameters.AddWithValue("isActive", 1);
                    cmd.Parameters.AddWithValue("username", username);

                    using var rdr = await cmd.ExecuteReaderAsync();
                    if (await rdr.ReadAsync())
                    {
                        return new ObjectException<IUser>(new User
                        {
                            Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                            Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                            Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                            Role = Helpers.ConvertFromDBVal<Role>(rdr["role"])
                        });
                    }
                }

                return new ObjectException<IUser>(null, null);
            }
            catch (Exception ex)
            {
                return new ObjectException<IUser>(ex);
            }
        }

        public static async Task<IObjectException<IUser>> GetUserByIdAsync(long userId)
        {
            try
            {
                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE id = @userId", conn);
                    cmd.Parameters.AddWithValue("userId", userId);

                    using var rdr = await cmd.ExecuteReaderAsync();
                    if (await rdr.ReadAsync())
                    {
                        return new ObjectException<IUser>(new User
                        {
                            Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                            Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                            Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                            Role = Helpers.ConvertFromDBVal<Role>(rdr["role"])
                        });
                    }
                }

                return new ObjectException<IUser>(null, null);
            }
            catch (Exception ex)
            {
                return new ObjectException<IUser>(ex);
            }
        }

        public static async Task<Exception> DeleteUserAsync(long userId)
        {
            try
            {
                using var conn = new MySqlConnection(DBAccess.ConnectionString());
                conn.Open();

                using var cmd = new MySqlCommand("UPDATE user SET isActive = @isActive WHERE id = @userId", conn);
                cmd.Parameters.AddWithValue("isActive", 0);
                cmd.Parameters.AddWithValue("userId", userId);

                await cmd.ExecuteNonQueryAsync();

                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static async Task<IObjectException<IUser>> UpdateUserPasswordAsync(UserUpdatePasswordRequest request, long userId)
        {
            IHashProvider hashProvider = new HashProvider();

            try
            {
                using (var conn = new MySqlConnection(DBAccess.ConnectionString()))
                {
                    conn.Open();

                    using var cmd = new MySqlCommand("UPDATE user SET password = @password WHERE id = @userId", conn);
                    cmd.Parameters.AddWithValue("password", hashProvider.ComputeHash(request.Password));
                    cmd.Parameters.AddWithValue("userId", userId);

                    await cmd.ExecuteNonQueryAsync();
                }

                return await GetUserByIdAsync(userId);
            }
            catch (Exception ex)
            {
                return new ObjectException<IUser>(ex);
            }
        }
    }
}

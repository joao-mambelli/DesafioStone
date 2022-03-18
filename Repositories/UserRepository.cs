using DesafioStone.Interfaces.RepositoriesInterfaces;
using DesafioStone.Models;
using DesafioStone.Enums;
using DesafioStone.Utils.Common;
using MySql.Data.MySqlClient;
using DesafioStone.Interfaces.ModelsInterfaces;

namespace DesafioStone.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<IEnumerable<IUser>> GetAllUsersAsync(bool active = true)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE isActive = @isActive", conn);
            cmd.Parameters.AddWithValue("isActive", active ? 1 : 0);

            var list = new List<IUser>();

            using var rdr = await cmd.ExecuteReaderAsync();
            while (await rdr.ReadAsync())
            {
                list.Add(new User
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                    Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                    Role = Helpers.ConvertFromDBVal<RoleEnum>(rdr["role"])
                });
            }

            return list;
        }

        public async Task<IUser> GetUserByIdAsync(long userId, bool active = true)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE id = @userId AND isActive = @isActive", conn);
            cmd.Parameters.AddWithValue("userId", userId);
            cmd.Parameters.AddWithValue("isActive", active ? 1 : 0);

            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new User
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                    Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                    Role = Helpers.ConvertFromDBVal<RoleEnum>(rdr["role"])
                };
            }

            return null;
        }

        public async Task<IUser> GetUserByUsernameAsync(string username, bool active = true)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("SELECT id, username, password, role FROM user WHERE username = @username AND isActive = @isActive", conn);
            cmd.Parameters.AddWithValue("username", username);
            cmd.Parameters.AddWithValue("isActive", active ? 1 : 0);

            using var rdr = await cmd.ExecuteReaderAsync();
            if (await rdr.ReadAsync())
            {
                return new User
                {
                    Id = Helpers.ConvertFromDBVal<long>(rdr["id"]),
                    Username = Helpers.ConvertFromDBVal<string>(rdr["username"]),
                    Password = Helpers.ConvertFromDBVal<string>(rdr["password"]),
                    Role = Helpers.ConvertFromDBVal<RoleEnum>(rdr["role"])
                };
            }

            return null;
        }

        public async Task<long> InsertUserAsync(IUser user)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("INSERT INTO user (username, password, role) VALUES (@username, @password, @role)", conn);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("role", (int)user.Role);

            await cmd.ExecuteNonQueryAsync();

            return cmd.LastInsertedId;
        }

        public async Task UpdateUserAsync(IUser user)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("UPDATE user SET username = @username, password, role = @password WHERE id = @userId", conn);
            cmd.Parameters.AddWithValue("username", user.Username);
            cmd.Parameters.AddWithValue("password", user.Password);
            cmd.Parameters.AddWithValue("userId", user.Id);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task DeleteUserAsync(long userId)
        {
            using var conn = new MySqlConnection(AccessDataBase.ConnectionString());
            conn.Open();

            using var cmd = new MySqlCommand("UPDATE user SET isActive = @isActive WHERE id = @userId", conn);
            cmd.Parameters.AddWithValue("isActive", 0);
            cmd.Parameters.AddWithValue("userId", userId);

            await cmd.ExecuteNonQueryAsync();
        }
    }
}

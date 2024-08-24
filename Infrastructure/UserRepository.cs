using Application.Interfaces;
using Domain.Entities;
using Npgsql;

namespace Infrastructure
{
    public class UserRepository : IRepository<User>
    {
        private DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            SELECT * FROM Users
            """;
            var command = new NpgsqlCommand(sql, connection);
            var users = new List<User>();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        Email = reader.GetString(4),
                        Role = (Role)reader.GetInt32(5),
                        PasswordHash = reader.GetString(6),
                    });
                }
            }

            return users;

        }

        public async Task<User> GetById(int id)
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            SELECT * FROM Users 
            WHERE Id = @id
            """;
            var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    FirstName = reader.GetString(2),
                    LastName = reader.GetString(3),
                    Email = reader.GetString(4),
                    Role = (Role)reader.GetInt32(5),
                    PasswordHash = reader.GetString(6),
                };
            }

            return new();

        }

        public async Task<User> Find(params object[] email)
        {

            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            SELECT * FROM Users 
            WHERE Email = @email
            """;
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("email", email[0]);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            FirstName = reader.GetString(2),
                            LastName = reader.GetString(3),
                            Email = reader.GetString(4),
                            Role = (Role)reader.GetInt32(5),
                            PasswordHash = reader.GetString(6),

                        };
                    }
                }


                return null;
            }


        }
        public async Task Create(User user)
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            INSERT INTO Users (Title, FirstName, LastName, Email, Role, PasswordHash)
            VALUES (@Title, @FirstName, @LastName, @Email, @Role, @PasswordHash)
            """;

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("Title", user.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("FirstName", user.FirstName ?? string.Empty);
                cmd.Parameters.AddWithValue("LastName", user.LastName ?? string.Empty);
                cmd.Parameters.AddWithValue("Email", user.Email ?? string.Empty);
                cmd.Parameters.AddWithValue("Role", (int)user.Role);
                cmd.Parameters.AddWithValue("PasswordHash", user.PasswordHash ?? string.Empty);


                cmd.ExecuteNonQuery();
            }




        }

        public async Task Update(User user)
        {
            using var connection = await _context.CreateConnectionAsync();

            var sql = """
            UPDATE Users 
            SET Title = @Title,
                FirstName = @FirstName,
                LastName = @LastName, 
                Email = @Email, 
                Role = @Role, 
                PasswordHash = @PasswordHash
            WHERE Id = @Id
          """;

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;

                cmd.Parameters.AddWithValue("Title", user.Title ?? string.Empty);
                cmd.Parameters.AddWithValue("FirstName", user.FirstName ?? string.Empty);
                cmd.Parameters.AddWithValue("LastName", user.LastName ?? string.Empty);
                cmd.Parameters.AddWithValue("Email", user.Email ?? string.Empty);
                cmd.Parameters.AddWithValue("Role", user.Role);
                cmd.Parameters.AddWithValue("PasswordHash", user.PasswordHash ?? string.Empty);
                cmd.Parameters.AddWithValue("Id", user.Id);




                cmd.ExecuteNonQuery();
            }


        }

        public async Task Delete(int id)
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            DELETE FROM Users 
            WHERE Id = @id
          """;
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("id", id);

                cmd.ExecuteNonQuery();
            }


        }


    }
}

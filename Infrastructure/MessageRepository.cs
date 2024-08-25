using Application.Interfaces;
using Domain.Entities;
using Npgsql;

namespace Infrastructure
{
    public class MessageRepository : IRepository<Message>
    {
        private DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAll()
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            SELECT * FROM Message
        """;
            var command = new NpgsqlCommand(sql, connection);
            var users = new List<Message>();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    byte[] data = (byte[])reader["Text"];
                    users.Add(new Message
                    {
                        Id = reader.GetInt32(0),
                        Text = data,
                        
                    });
                }
            }

            return users;

        }

        public async Task<Message> GetById(int id)
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            SELECT * FROM Message 
            WHERE Id = @id
        """;
            var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                byte[] data = (byte[])reader["Text"];
                return new Message
                {
                    Id = reader.GetInt32(0),
                    Text = data,
                   
                };
            }

            return new();

        }

        public async Task<Message> Find(params object[] date)
        {

            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            SELECT * FROM Users 
            WHERE Date = @date
            """;
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("date", date[0]);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] data = (byte[])reader["Text"];
                        return new Message
                        {
                            Id = reader.GetInt32(0),

                            Text = data,
                          

                        };
                    }
                }


                return null;
            }


        }
        public async Task Create(Message message)
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            INSERT INTO Message (Text, Date)
            VALUES (@Text, @Date)
            """;

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("Text", message.Text);
                cmd.Parameters.AddWithValue("Date", message.Date);
               


                cmd.ExecuteNonQuery();
            }




        }

        public async Task Update(Message message)
        {
            using var connection = await _context.CreateConnectionAsync();

            var sql = """
            UPDATE Message 
            SET Text = @Text,
                Date = @Date
               
            WHERE Id = @Id
            """;

            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("Id", message.Id);
                cmd.Parameters.AddWithValue("Text", message.Text);
                cmd.Parameters.AddWithValue("Date", message.Date);
                cmd.ExecuteNonQuery();
            }


        }

        public async Task Delete(int id)
        {
            using var connection = await _context.CreateConnectionAsync();
            var sql = """
            DELETE FROM Message
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

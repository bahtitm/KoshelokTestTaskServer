using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Infrastructure
{
    public class DataContext
    {
        private DbSettings _dbSettings;
        private NpgsqlDataSource npgsqlDataSource;
        private readonly ILogger<DataContext> logger;


        public DataContext(IOptions<DbSettings> dbSettings, ILogger<DataContext> logger)
        {
            _dbSettings = dbSettings.Value;
            this.logger = logger;
            var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
            this.logger.LogDebug(connectionString); ;
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            npgsqlDataSource = dataSourceBuilder.Build();
        }

        public async Task<NpgsqlConnection> CreateConnectionAsync()
        {
            return await npgsqlDataSource.OpenConnectionAsync();

        }

        public async Task Init()
        {
            await _initDatabase();
            await _initTables();
        }

        private async Task _initDatabase()
        {
            var connectionString = $"Host={_dbSettings.Server}; Database=postgres; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
            try
            {

                using var conn = new NpgsqlConnection(connectionString);
                logger.LogDebug(connectionString);
                await conn.OpenAsync();

                string checkDBExistsQuery = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";

                string createDBQuery = $"CREATE DATABASE \"{_dbSettings.Database}\"";
                long tableExists;
                using (var cmd = new NpgsqlCommand(checkDBExistsQuery, conn))
                {

                    tableExists = (long)cmd.ExecuteScalar();
                }
                if (tableExists == 0)
                {
                    using (var cmd = new NpgsqlCommand(createDBQuery, conn))
                    {
                        cmd.ExecuteNonQuery();

                    }
                }

            }
            catch (Exception)
            {
                logger.LogError($"connectionstrin: {connectionString}");
                //throw;
            }



        }

        private async Task _initTables()
        {
            await _initUsers();
            await _initMassage();
        }
        private async Task _initMassage()
        {
            var conn = await npgsqlDataSource.OpenConnectionAsync();
            var sql = """
                CREATE TABLE IF NOT EXISTS Message (
                    Id SERIAL PRIMARY KEY,
                    Number INTEGER,
                    Text BYTEA,
                    Date TIMESTAMP
                   
                );
            """;


            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();

            }
        }
        async Task _initUsers()
        {

            var conn = await npgsqlDataSource.OpenConnectionAsync();
            var sql = """
                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Title VARCHAR,
                    FirstName VARCHAR,
                    LastName VARCHAR,
                    Email VARCHAR,
                    Role INTEGER,
                    PasswordHash VARCHAR
                );
            """;


            using (var cmd = new NpgsqlCommand(sql, conn))
            {
                cmd.ExecuteNonQuery();

            }



        }
    }
}

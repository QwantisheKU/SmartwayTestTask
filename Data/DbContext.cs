using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace Data
{
	public class DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration["ConnectionStrings:DbConnection"];
        }
        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}

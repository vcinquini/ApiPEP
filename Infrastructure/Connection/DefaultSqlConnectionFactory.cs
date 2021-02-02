using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Connection
{
    public class DefaultSqlConnectionFactory : ISQLConnectionFactory
    {
        private readonly IConfiguration _config;

        public DefaultSqlConnectionFactory(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }
    }
}

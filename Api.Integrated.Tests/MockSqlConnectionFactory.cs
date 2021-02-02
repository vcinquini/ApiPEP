using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Api.Integrated.Tests
{
    public class MockSqlConnectionFactory : ISQLConnectionFactory
    {
        private readonly IConfiguration _config;

        public MockSqlConnectionFactory()
        {
            _config = GetConfiguration();
        }

        private static IConfiguration GetConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("AppSettings.json");
            IConfiguration config = configurationBuilder.Build();
            return config;
        }

        public IDbConnection Connection()
        {
            string dataBaseName = _config.GetConnectionString("IntegrationTestConnection");
            return new SqlConnection(dataBaseName);
        }
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Api.Integrated.Tests
{
    public static class DatabaseBootstrap
    {
        private static readonly IDbConnection _conn;
		private static readonly IConfiguration _config;

		static DatabaseBootstrap()
        {
            _config = GetConfiguration();
            string dataBaseName = _config.GetConnectionString("DBBootstrapConnection");
            _conn = new SqlConnection(dataBaseName);
        }

        public static void Setup()
        {
            using (_conn)
            {
                string[] script = GetScript().Split("GO", System.StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in script)
                {
                    _conn.Execute(item);
                }
			}
        }

		private static IConfiguration GetConfiguration()
		{
			IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddJsonFile("AppSettings.json");
			IConfiguration config = configurationBuilder.Build();
			return config;
		}


		private static string GetScript()
        {
            return @"USE [MASTER]

					IF NOT EXISTS(SELECT 1 FROM SYS.databases WHERE NAME LIKE '%persons_test%')
					BEGIN
						EXEC('CREATE DATABASE [persons_test]');
					END
					GO

					USE [persons_test]

					IF NOT EXISTS(SELECT * FROM sys.tables WHERE type = 'U' AND name like 'Personn')
					BEGIN
						CREATE TABLE Persons(CPF bigint NOT NULL, CreateAt DateTime NULL, ChangedAt DateTime NULL, PEP int NULL, PEPType int NULL, );
						INSERT INTO Persons VALUES(11111111111, getdate(), getdate(), 1, 1);
					END

					if NOT EXISTS(SELECT * FROM SYS.procedures WHERE type = 'P' AND NAME LIKE '%SearchPerson%')
					BEGIN
						EXEC('CREATE OR ALTER PROCEDURE [dbo].[SearchPerson] 
						(
							@CPF bigint
						)
						AS
						BEGIN
							SELECT
								p.CPF,
								P.CreateAt,
								P.ChangedAt,
								P.PEP,
								P.PEPType
							FROM
								Persond P
							WHERE
								P.CPF = @CPF
						END');
					END
					GO";

		}
    }
}

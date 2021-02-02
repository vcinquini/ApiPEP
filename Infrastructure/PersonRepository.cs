using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccess
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ILogger<PersonRepository> _logger;
        private readonly ISQLConnectionFactory _conn;
        private readonly IConfiguration _configuration;

        public PersonRepository(ILogger<PersonRepository> logger,
                                ISQLConnectionFactory connection,
                                IConfiguration configuration)
        {
            _logger = logger;
            _conn = connection;
            _configuration = configuration;
        }


        public async Task<Person> GetByIdAsync(long id)
        {
            Person Person = null;

            var policy = Policy.Handle<Exception>().WaitAndRetry(
                   Convert.ToInt32(_configuration["Polly:MaxRetry"]),
                    retryCount => TimeSpan.FromSeconds(Math
                    .Pow(retryCount, 2)), (ex, time, count, context) =>
            {
                _logger.LogError(ex, $"An error occured while searching for a Person. Error: {ex.Message}. Try #{count}");
                Person = null;
            });

            await policy.Execute(async () =>
            {
                dynamic result = null;

                using (var connectionDb = _conn.Connection())
                {
                    connectionDb.Open();

                    try
                    {
                        DynamicParameters param = new DynamicParameters();
                        param.Add("@CPF", id);

                        result = await connectionDb.QueryFirstAsync<dynamic>("SearchPerson", param, commandType: CommandType.StoredProcedure);

                        if (result != null)
                        {
                            long cpf = Convert.ToInt64(result.CPF);
                            DateTime dt = Convert.ToDateTime(result.CreatedAt);
                            DateTime da = Convert.ToDateTime(result.ChangedAt);
                            int pep = Convert.ToInt32(result.PEP);
                            int tppep = Convert.ToInt32(result.TipoPEP);

                            Person = new Person(cpf, dt, da, pep, tppep);
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        Person = null;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            });

            return Person;

        }

        public Task<bool> AddAsync(Person entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Person>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Person entity)
        {
            throw new NotImplementedException();
        }
    }
}

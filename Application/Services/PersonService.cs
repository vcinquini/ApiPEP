using Domain.Entities;
using Domain.Repository;
using Domain.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly ILogger<PersonService> _logger;
        private readonly IPersonRepository _personRepository;

        public PersonService(ILogger<PersonService> logger, IPersonRepository personRepository)
        {
            _personRepository = personRepository;
            _logger = logger;
        }

        public async Task<Person> GetPerson(long Cpf)
        {
            Person person = null;
            try
            {
                person = await _personRepository.GetByIdAsync(Cpf);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting Person CPF {Cpf}. Error: {ex.Message}");
            }
            return person;
        }
    }
}
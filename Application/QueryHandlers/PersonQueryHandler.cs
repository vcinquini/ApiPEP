using Application.Aplicacao.Queries;
using Domain.Entities;
using Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.QueryHandlers
{
    public class PersonQueryHandler : IRequestHandler<PersonQuery, Person>
    {
        private readonly IPersonService _pessoaService;

        public PersonQueryHandler(IPersonService pessoaService)
        {
            _pessoaService = pessoaService;
        }


        public async Task<Person> Handle(PersonQuery request, CancellationToken cancellationToken)
        {
            return await _pessoaService.GetPerson(request.CPF);
        }
    }
}

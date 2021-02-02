using Domain.Entities;
using MediatR;


namespace Application.Aplicacao.Queries
{
    public sealed class PersonQuery : IRequest<Person>
    {
        public long CPF { get; set; }

        public PersonQuery(long cpf)
        {
            CPF = cpf;
        }
    }
}

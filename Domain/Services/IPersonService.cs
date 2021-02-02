using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Services
{
    public interface IPersonService
    {
        Task<Person> GetPerson(long Cpf);
    }
}
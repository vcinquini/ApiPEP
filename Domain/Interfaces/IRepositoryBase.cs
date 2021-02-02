using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<bool> AddAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(long id);
    }
}


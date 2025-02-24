using DHP_Manager.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DHP_Manager.Repositories.Interface
{
    public interface ICustomerRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByUniqueIdentifierAsync(string identifier);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}

using DHP_Manager.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DHP_Manager.Repositories.Interface
{
    public interface INormalCustomerRepository
    {
        Task<IEnumerable<NormalCustomer>> GetAllAsync();
        Task<NormalCustomer> GetByPhoneNumberAsync(string phoneNumber);
        Task AddAsync(NormalCustomer customer);
        Task UpdateAsync(NormalCustomer customer);
        Task DeleteAsync(int id);
    }
}

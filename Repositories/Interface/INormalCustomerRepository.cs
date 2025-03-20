using DHP_Manager.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DHP_Manager.Repositories.Interface
{
    public interface INormalCustomerRepository
    {
        Task<IEnumerable<NormalCustomer>> GetAllAsync();
        Task<NormalCustomer> GetByPhoneNumberAsync(string phone);
        Task<NormalCustomer> AddAsync(NormalCustomer entity);
        Task<NormalCustomer> UpdateAsync(string phone, NormalCustomer dto);
        Task DeleteAsync(int id);
        Task<NormalCustomer> GetByIdAsync(int id);
    }
}

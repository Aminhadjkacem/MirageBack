using DHP_Manager.Models.Details;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Input;

namespace DHP_Manager.Services.Interface
{
    public interface INormalCustomerService
    {
        Task<IEnumerable<NormalCustomerDetails>> GetAllAsync();
        Task<NormalCustomerDetails> GetByPhoneAsync(string phone);
        Task<NormalCustomerDetails> AddAsync(NormalCustomerInput dto);
        Task<NormalCustomerDetails> UpdateAsync(string phone, NormalCustomerInput dto);
        Task<bool> DeleteAsync(string phone);
    }
}

using DHP_Manager.Models.DTOs;

namespace DHP_Manager.Services.Interface
{
    public interface INormalCustomerService
    {
        Task<IEnumerable<NormalCustomerDTO>> GetAllAsync();
        Task<NormalCustomerDTO> GetByPhoneAsync(string phone);
        Task<NormalCustomerDTO> AddAsync(NormalCustomerDTO dto);
        Task<NormalCustomerDTO> UpdateAsync(string phone, NormalCustomerDTO dto);
        Task<bool> DeleteAsync(string phone);
    }
}

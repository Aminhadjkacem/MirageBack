using DHP_Manager.Models.Entities;

namespace DHP_Manager.Repositories.Interfaces
{
    public interface IQuotationRepository
    {
        Task<IEnumerable<Quotation>> GetAllAsync();
        Task<Quotation?> GetByIdAsync(int id);
        Task<Quotation> AddAsync(Quotation quotation);
        Task<Quotation> UpdateAsync(Quotation quotation);
        Task<bool> DeleteAsync(int id);
    }
}

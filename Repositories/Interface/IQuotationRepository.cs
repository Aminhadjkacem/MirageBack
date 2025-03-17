using DHP_Manager.Models.Entities;

namespace DHP_Manager.Repositories.Interface
{
    public interface IQuotationRepository
    {
        Task<IEnumerable<Quotation>> GetAllAsync(); // Update to include designations
        Task<Quotation> GetByIdAsync(int id);
        Task<Quotation> GetQuotationWithDesignationsAsync(int id);
        Task AddAsync(Quotation quotation);
        Task UpdateAsync(Quotation quotation);
        Task DeleteAsync(int id);
    }
}

using DHP_Manager.Models.Entities;

namespace DHP_Manager.Repositories.Interface
{
    public interface IDesignationRepository
    {
        Task<Designation> GetByIdAsync(int id);
        Task<IEnumerable<Designation>> GetAllAsync();
        Task<Designation> GetDesignationWithProductsAsync(int id);
        Task<IEnumerable<Designation>> GetAllByQuotationIdAsync(int quotationId);
        Task AddAsync(Designation designation);
        Task UpdateAsync(Designation designation);
        Task DeleteAsync(int id);
    }
}

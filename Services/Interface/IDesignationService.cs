using DHP_Manager.Models.Entities;

namespace DHP_Manager.Services.Interface
{
    public interface IDesignationService
    {
        Task<Designation> CreateDesignationAsync(int quotationId, string name);
        Task<Designation> GetDesignationAsync(int id);
        Task<IEnumerable<Designation>> GetDesignationsByQuotationAsync(int quotationId);
        Task<Designation> GetDesignationWithProductsAsync(int id);
        Task UpdateDesignationAsync(Designation designation);
        Task DeleteDesignationAsync(int id);
        Task AddProductToDesignationAsync(int designationId, int productId, int quantity);
    }
}

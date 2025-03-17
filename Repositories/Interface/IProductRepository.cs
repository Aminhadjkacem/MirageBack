using DHP_Manager.Models.Entities;

namespace DHP_Manager.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByNameAsync(string name);
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetProductWithDesignationsAsync(int id); // Added to fetch designations
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<Dictionary<int, Product>> GetProductsByIdsAsync(IEnumerable<int> productIds);
        Task UpdateProductStocksAsync(Dictionary<int, int> productQuantities);
    }
}
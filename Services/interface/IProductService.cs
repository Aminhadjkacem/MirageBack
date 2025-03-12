using DHP_Manager.Models.Details;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;

namespace DHP_Manager.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDetail>> GetAllProductsAsync();
        Task<ProductDetail> GetProductByIdAsync(int id);
        Task<ProductDetail> GetProductByNameAsync(string name);
        Task AddProductAsync(ProductInput productInput);
        Task UpdateProductAsync(int id, ProductInput productInput);
        Task PatchProductAsync(int id, ProductInput productPatchDto);
        Task DeleteProductAsync(int id);
    }
}

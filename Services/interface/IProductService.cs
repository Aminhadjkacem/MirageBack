using DHP_Manager.Models.Details;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Input;
using DHP_Manager.Models.response;

namespace DHP_Manager.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDetail>> GetAllProductsAsync();
        Task<ProductDetail> GetProductByIdAsync(int id);
        Task<ProductDetail> GetProductByNameAsync(string name);
        Task<ProductDetail> GetProductWithDesignationsAsync(int id); // New method to get product with designations
        Task AddProductAsync(ProductInput productInput);
        Task UpdateProductAsync(int id, ProductInput productInput);
        Task PatchProductAsync(int id, ProductInput productPatchDto);
        Task DeleteProductAsync(int id);
        Task UpdateProductStocksAsync(Dictionary<int, int> productQuantities);
        Task AddProductToDesignationAsync(int productId, int designationId, int quantity); // New method
        Task RemoveProductFromDesignationAsync(int productId, int designationId); // New method
        Task<bool> VerifyStockAsync(List<ProductQuantityInput> products); // New overload
        Task VerifyAndUpdateStockAsync(List<ProductQuantityInput> products); // New method
    }
}

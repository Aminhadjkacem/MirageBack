using DHP_Manager.Helpers.Exceptions;
using DHP_Manager.Models.Details;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Input; // Changed to match updated namespace
using DHP_Manager.Models.response;
using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Services.Impl // Changed 'impl' to 'Impl' to follow C# conventions
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IDesignationRepository _designationRepository; // Added for designation operations

        public ProductService(
            IProductRepository productRepository,
            IDesignationRepository designationRepository)
        {
            _productRepository = productRepository;
            _designationRepository = designationRepository;
        }

        public async Task<IEnumerable<ProductDetail>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => new ProductDetail
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Category = p.Category,
                Designations = p.DesignationProducts.Select(dp => dp.Designation.Name).ToList() // Include designation names
            });
        }

        public async Task<ProductDetail> GetProductByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return null;

            return new ProductDetail
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Designations = product.DesignationProducts.Select(dp => dp.Designation.Name).ToList()
            };
        }

        public async Task<ProductDetail> GetProductByNameAsync(string name)
        {
            var product = await _productRepository.GetByNameAsync(name);
            if (product == null)
                return null;

            return new ProductDetail
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Designations = product.DesignationProducts.Select(dp => dp.Designation.Name).ToList()
            };
        }

        public async Task<ProductDetail> GetProductWithDesignationsAsync(int id)
        {
            var product = await _productRepository.GetProductWithDesignationsAsync(id);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return new ProductDetail
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                Category = product.Category,
                Designations = product.DesignationProducts.Select(dp => dp.Designation.Name).ToList()
            };
        }

        public async Task AddProductAsync(ProductInput productInput)
        {
            var product = productInput.ToEntity(productInput); // Assuming ToEntity is an extension method
            product.DesignationProducts = new List<DesignationProduct>();
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProductAsync(int id, ProductInput productInput)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException("Product not found.");

            existingProduct.Name = productInput.Name;
            existingProduct.Description = productInput.Description;
            existingProduct.Price = productInput.Price;
            existingProduct.Stock = productInput.Stock;
            existingProduct.Category = productInput.Category;

            await _productRepository.UpdateAsync(existingProduct);
        }

        public async Task PatchProductAsync(int id, ProductInput productPatchDto)
        {
            if (productPatchDto == null)
                throw new ArgumentNullException(nameof(productPatchDto));

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException("Product not found.");

            if (!string.IsNullOrEmpty(productPatchDto.Name))
                existingProduct.Name = productPatchDto.Name;
            if (productPatchDto.Description != null)
                existingProduct.Description = productPatchDto.Description;
            if (productPatchDto.Price != default)
                existingProduct.Price = productPatchDto.Price;
            if (productPatchDto.Stock != default)
                existingProduct.Stock = productPatchDto.Stock;
            if (!string.IsNullOrEmpty(productPatchDto.Category))
                existingProduct.Category = productPatchDto.Category;

            await _productRepository.UpdateAsync(existingProduct);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productRepository.GetProductWithDesignationsAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            if (product.DesignationProducts.Any())
                throw new InvalidOperationException("Cannot delete product that is associated with designations.");

            await _productRepository.DeleteAsync(id);
        }


        public async Task UpdateProductStocksAsync(Dictionary<int, int> productQuantities)
        {
            var productIds = productQuantities.Keys.ToList();
            var products = await _productRepository.GetProductsByIdsAsync(productIds);

            foreach (var product in products.Values)
            {
                if (productQuantities.TryGetValue(product.Id, out int quantityToDeduct))
                {
                    if (product.Stock < quantityToDeduct)
                        throw new InsufficientStockException(
                            $"Cannot deduct {quantityToDeduct} from product '{product.Name}'. Current stock: {product.Stock}");

                    product.Stock -= quantityToDeduct;
                }
            }

            foreach (var product in products.Values)
            {
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task AddProductToDesignationAsync(int productId, int designationId, int quantity)
        {
            var product = await _productRepository.GetProductWithDesignationsAsync(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var designation = await _designationRepository.GetByIdAsync(designationId);
            if (designation == null)
                throw new KeyNotFoundException($"Designation with ID {designationId} not found.");

            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            var designationProduct = new DesignationProduct
            {
                ProductId = productId,
                DesignationId = designationId,
                Quantity = quantity
            };

            product.DesignationProducts.Add(designationProduct);
            await _productRepository.UpdateAsync(product);
        }

        public async Task RemoveProductFromDesignationAsync(int productId, int designationId)
        {
            var product = await _productRepository.GetProductWithDesignationsAsync(productId);
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {productId} not found.");

            var designationProduct = product.DesignationProducts
                .FirstOrDefault(dp => dp.DesignationId == designationId);

            if (designationProduct == null)
                throw new InvalidOperationException(
                    $"Product {productId} is not associated with Designation {designationId}.");

            product.DesignationProducts.Remove(designationProduct);
            await _productRepository.UpdateAsync(product);
        }
        public async Task<bool> VerifyStockAsync(List<ProductQuantityInput> products)
        {
            if (products == null || !products.Any())
                throw new ArgumentException("Product list cannot be null or empty.", nameof(products));

            var productIds = products.Select(p => p.ProductId).Distinct().ToList();
            var productEntities = await _productRepository.GetProductsByIdsAsync(productIds);

            var requestedQuantities = products
                .GroupBy(p => p.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(p => p.Quantity)
                });

            foreach (var request in requestedQuantities)
            {
                if (!productEntities.TryGetValue(request.ProductId, out var product))
                    throw new InsufficientStockException($"Product with ID {request.ProductId} not found.");

                if (request.TotalQuantity > product.Stock)
                    throw new InsufficientStockException(
                        $"Insufficient stock for product '{product.Name}': " +
                        $"Requested {request.TotalQuantity}, Available {product.Stock}");
            }

            return true;
        }
    }
}
using DHP_Manager.Models.Details;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;
using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.Interface;

namespace DHP_Manager.Services.impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // Method to fetch all products and return as ProductDetail
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
                Category = p.Category
            });
        }

        // Method to get a product by ID and return it as ProductDetail
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
                Category = product.Category
            };
        }

        // Method to get a product by name and return it as ProductDetail
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
                Category = product.Category
            };
        }

        // Method to add a new product using ProductInput
        public async Task AddProductAsync(ProductInput productInput)
        {
            // Map ProductInput to Product entity
            var product = productInput.ToEntity(productInput);
            await _productRepository.AddAsync(product);
        }

        // Method to update an existing product by ID using ProductInput
        public async Task UpdateProductAsync(int id, ProductInput productInput)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException("Product not found.");

            // Update properties of existing product
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

            // Update only the fields that are provided in the patch DTO
            if (!string.IsNullOrEmpty(productPatchDto.Name))
                existingProduct.Name = productPatchDto.Name;
            if (productPatchDto.Description != null) // Nullable string, so null check is fine
                existingProduct.Description = productPatchDto.Description;
            if (productPatchDto.Price != default) // Check if Price has been explicitly set
                existingProduct.Price = productPatchDto.Price;
            if (productPatchDto.Stock != default) // Check if Stock has been explicitly set
                existingProduct.Stock = productPatchDto.Stock;
            if (!string.IsNullOrEmpty(productPatchDto.Category))
                existingProduct.Category = productPatchDto.Category;

            await _productRepository.UpdateAsync(existingProduct);
        }

        // Method to delete a product by ID
        public async Task DeleteProductAsync(int id)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new KeyNotFoundException("Product not found.");

            await _productRepository.DeleteAsync(id);
        }
    }
}

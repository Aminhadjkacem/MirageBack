using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories.Impl
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.DesignationProducts) // Include related designations
                .ThenInclude(dp => dp.Designation)
                .ToListAsync();
        }

        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.DesignationProducts)
                .ThenInclude(dp => dp.Designation)
                .FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.DesignationProducts)
                .ThenInclude(dp => dp.Designation)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> GetProductWithDesignationsAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.DesignationProducts)
                .ThenInclude(dp => dp.Designation)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            return product;
        }

        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetProductWithDesignationsAsync(id); // Use method that includes designations
            if (product == null)
                throw new KeyNotFoundException($"Product with ID {id} not found.");

            if (product.DesignationProducts.Any())
                throw new InvalidOperationException(
                    $"Cannot delete product '{product.Name}' as it is associated with {product.DesignationProducts.Count} designation(s).");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<int, Product>> GetProductsByIdsAsync(IEnumerable<int> productIds)
        {
            return await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Include(p => p.DesignationProducts) // Include designations for consistency
                .ThenInclude(dp => dp.Designation)
                .ToDictionaryAsync(p => p.Id, p => p);
        }

        public async Task UpdateProductStocksAsync(Dictionary<int, int> productQuantities)
        {
            var productIds = productQuantities.Keys.ToList();
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync(); // No need to include designations here unless required

            foreach (var product in products)
            {
                if (productQuantities.TryGetValue(product.Id, out int quantityToDeduct))
                {
                    if (product.Stock < quantityToDeduct)
                        throw new InvalidOperationException(
                            $"Cannot deduct {quantityToDeduct} from product '{product.Name}'. Current stock: {product.Stock}");

                    product.Stock -= quantityToDeduct;
                }
            }

            _context.Products.UpdateRange(products);
            await _context.SaveChangesAsync();
        }
    }
}
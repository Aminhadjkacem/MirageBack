using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories.Impl
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly ApplicationDbContext _context;

        public QuotationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Quotation>> GetAllAsync()
        {
            return await _context.Quotations
                .Include(q => q.Designations)
                .ThenInclude(d => d.DesignationProducts)
                .ThenInclude(dp => dp.Product) // Include Product to get ProductName
                .ToListAsync();
        }

        public async Task<Quotation> GetByIdAsync(int id)
        {
            return await _context.Quotations
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Quotation> GetQuotationWithDesignationsAsync(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Designations)
                .ThenInclude(d => d.DesignationProducts)
                .ThenInclude(dp => dp.Product)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                throw new KeyNotFoundException($"Quotation with ID {id} not found.");

            return quotation;
        }

        public async Task AddAsync(Quotation quotation)
        {
            await _context.Quotations.AddAsync(quotation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Quotation quotation)
        {
            var existingQuotation = await _context.Quotations
                .Include(q => q.Designations)
                .ThenInclude(d => d.DesignationProducts)
                .FirstOrDefaultAsync(q => q.Id == quotation.Id);

            if (existingQuotation == null)
                throw new KeyNotFoundException($"Quotation with ID {quotation.Id} not found");

            // Update Quotation properties
            _context.Entry(existingQuotation).CurrentValues.SetValues(quotation);

            // Handle Designations
            if (quotation.Designations != null)
            {
                foreach (var designationInput in quotation.Designations)
                {
                    var existingDesignation = existingQuotation.Designations
                        .FirstOrDefault(d => d.Id == designationInput.Id);

                    if (existingDesignation != null)
                    {
                        // Update existing designation
                        _context.Entry(existingDesignation).CurrentValues.SetValues(designationInput);
                    }
                    else
                    {
                        // Add new designation
                        designationInput.QuotationId = quotation.Id; // Ensure FK is set
                        existingQuotation.Designations.Add(designationInput);
                    }

                    // Handle DesignationProducts
                    if (designationInput.DesignationProducts != null)
                    {
                        var existingProducts = existingDesignation?.DesignationProducts ?? new List<DesignationProduct>();

                        foreach (var productInput in designationInput.DesignationProducts)
                        {
                            var existingProduct = existingProducts
                                .FirstOrDefault(p => p.ProductId == productInput.ProductId);

                            if (existingProduct != null)
                            {
                                existingProduct.Quantity = productInput.Quantity;
                            }
                            else
                            {
                                productInput.DesignationId = designationInput.Id;
                                existingQuotation.Designations
                                    .First(d => d.Id == designationInput.Id)
                                    .DesignationProducts.Add(productInput);
                            }
                        }

                        // Remove products no longer in input
                        var inputProductIds = designationInput.DesignationProducts.Select(p => p.ProductId).ToList();
                        var productsToRemove = existingProducts
                            .Where(p => !inputProductIds.Contains(p.ProductId))
                            .ToList();
                        foreach (var product in productsToRemove)
                        {
                            existingDesignation?.DesignationProducts.Remove(product);
                        }
                    }
                }

                // Remove designations no longer in input
                var inputDesignationIds = quotation.Designations.Select(d => d.Id).ToList();
                var designationsToRemove = existingQuotation.Designations
                    .Where(d => !inputDesignationIds.Contains(d.Id))
                    .ToList();
                foreach (var designation in designationsToRemove)
                {
                    existingQuotation.Designations.Remove(designation);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation != null)
            {
                _context.Quotations.Remove(quotation);
                await _context.SaveChangesAsync();
            }
        }
    }
}

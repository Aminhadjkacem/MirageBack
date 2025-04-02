using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.ENUM;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
                        .ThenInclude(dp => dp.Product)
                .Include(q => q.NormalCustomer)
                .Include(q => q.CompanyCustomer)
                .ToListAsync();
        }

        public async Task<Quotation> GetByIdAsync(int id)
        {
            return await _context.Quotations
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<int?> GetFactureIdAsync(int id)
        {
            var quotation = await _context.Quotations.FirstOrDefaultAsync(q => q.Id == id);
            return quotation?.LastInvoiceId;
        }


        public async Task UpdateQuotationStatusAsync(int id, QuotationStatus status)
        {
            var quotation = await _context.Quotations
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                throw new KeyNotFoundException($"Quotation with ID {id} not found.");

            quotation.Status = status;
            quotation.InvoiceCreatedAt = DateTime.UtcNow;

            var maxInvoiceId = await _context.Quotations
                .Where(q => q.LastInvoiceId.HasValue)
                .MaxAsync(q => (int?)q.LastInvoiceId) ?? 0; // Get max LastInvoiceId or default to 0

            quotation.LastInvoiceId = maxInvoiceId + 1;

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetFactureIdAsync()
        {
            var maxInvoiceId = await _context.Quotations
                .Where(q => q.LastInvoiceId.HasValue)
                .MaxAsync(q => q.LastInvoiceId);

            if (maxInvoiceId == null)
            {
                throw new InvalidOperationException("No invoices found.");
            }

            return maxInvoiceId.Value;
        }


        public async Task<List<Quotation>> GetValidatedQuotationsAsync()
        {
            return await _context.Quotations
                .Where(q => q.Status == QuotationStatus.VALIDATED)
                .Include(q => q.Designations)
                    .ThenInclude(d => d.DesignationProducts)
                        .ThenInclude(dp => dp.Product)
                .Include(q => q.NormalCustomer)
                .Include(q => q.CompanyCustomer)
                .ToListAsync();
        }

        public async Task<Quotation> GetQuotationWithDesignationsAsync(int id)
        {
            var quotation = await _context.Quotations
                .Include(q => q.Designations)
                    .ThenInclude(d => d.DesignationProducts)
                        .ThenInclude(dp => dp.Product)
                .Include(q => q.NormalCustomer)
                .Include(q => q.CompanyCustomer)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quotation == null)
                throw new KeyNotFoundException($"Quotation with ID {id} not found.");

            return quotation;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
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

            _context.Entry(existingQuotation).CurrentValues.SetValues(quotation);

            if (quotation.Designations != null)
            {
                foreach (var designationInput in quotation.Designations)
                {
                    var existingDesignation = existingQuotation.Designations
                        .FirstOrDefault(d => d.Id == designationInput.Id);

                    if (existingDesignation != null)
                    {
                        _context.Entry(existingDesignation).CurrentValues.SetValues(designationInput);
                    }
                    else
                    {
                        designationInput.QuotationId = quotation.Id;
                        existingQuotation.Designations.Add(designationInput);
                    }

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

                var inputDesignationIds = quotation.Designations.Select(d => d.Id).ToList();
                var designationsToRemove = existingQuotation.Designations
                    .Where(d => !inputDesignationIds.Contains(d.Id))
                    .ToList();
                foreach (var designation in designationsToRemove)
                {
                    existingQuotation.Designations.Remove(designation);
                }
            }

            existingQuotation.LastInvoiceId = quotation.LastInvoiceId;
            existingQuotation.InvoiceCreatedAt = quotation.InvoiceCreatedAt;
            existingQuotation.QuotationCreatedAt = quotation.QuotationCreatedAt;

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

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
            _context.Quotations.Update(quotation);
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

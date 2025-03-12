using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories
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
                .Include(q => q.QuotationProducts)
                .ThenInclude(qp => qp.Product)
                .ToListAsync();
        }

        public async Task<Quotation?> GetByIdAsync(int id)
        {
            return await _context.Quotations
                .Include(q => q.QuotationProducts)
                .ThenInclude(qp => qp.Product)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Quotation> AddAsync(Quotation quotation)
        {
            _context.Quotations.Add(quotation);
            await _context.SaveChangesAsync();
            return quotation;
        }

        public async Task<Quotation> UpdateAsync(Quotation quotation)
        {
            _context.Quotations.Update(quotation);
            await _context.SaveChangesAsync();
            return quotation;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var quotation = await _context.Quotations.FindAsync(id);
            if (quotation == null) return false;

            _context.Quotations.Remove(quotation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

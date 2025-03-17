using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories.Impl
{
    public class DesignationRepository : IDesignationRepository
    {
        private readonly ApplicationDbContext _context;

        public DesignationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Designation> GetByIdAsync(int id)
        {
            return await _context.Designations.FindAsync(id)
                ?? throw new KeyNotFoundException($"Designation with id {id} not found");
        }

        public async Task<IEnumerable<Designation>> GetAllAsync()
        {
            return await _context.Designations.ToListAsync();
        }

        public async Task<Designation> GetDesignationWithProductsAsync(int id)
        {
            return await _context.Designations
                .Include(d => d.DesignationProducts)
                .ThenInclude(dp => dp.Product)
                .Include(d => d.Quotation)
                .FirstOrDefaultAsync(d => d.Id == id)
                ?? throw new KeyNotFoundException($"Designation with id {id} not found");
        }

        public async Task<IEnumerable<Designation>> GetAllByQuotationIdAsync(int quotationId)
        {
            return await _context.Designations
                .Where(d => d.QuotationId == quotationId)
                .Include(d => d.DesignationProducts)
                .ThenInclude(dp => dp.Product)
                .ToListAsync();
        }

        public async Task AddAsync(Designation designation)
        {
            await _context.Designations.AddAsync(designation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Designation designation)
        {
            _context.Designations.Update(designation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var designation = await GetByIdAsync(id);
            _context.Designations.Remove(designation);
            await _context.SaveChangesAsync();
        }
    }
}

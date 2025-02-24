using DHP_Manager.Data;
using DHP_Manager.Entities;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories.Impl
{
    public class CompanyCustomerRepository : ICustomerRepository<CompanyCustomer> // Correctly specify the type here
    {
        private readonly ApplicationDbContext _context;

        public CompanyCustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyCustomer>> GetAllAsync()
        {
            return await _context.CompanyCustomers.ToListAsync();
        }

        public async Task<CompanyCustomer> GetByUniqueIdentifierAsync(string identifier)
        {
            return await _context.CompanyCustomers
                                 .FirstOrDefaultAsync(c => c.MatriculeFiscal == identifier);
        }
        public async Task AddAsync(CompanyCustomer customer)
        {
            await _context.CompanyCustomers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CompanyCustomer customer)
        {
            _context.CompanyCustomers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _context.CompanyCustomers.FindAsync(id);
            if (customer != null)
            {
                _context.CompanyCustomers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}

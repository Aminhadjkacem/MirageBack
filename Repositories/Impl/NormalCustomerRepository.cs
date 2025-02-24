using DHP_Manager.Data;
using DHP_Manager.Entities;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories.Impl
{

    public class NormalCustomerRepository : ICustomerRepository<NormalCustomer>
    {
        private readonly ApplicationDbContext _context;

        public NormalCustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NormalCustomer>> GetAllAsync() =>
            await _context.NormalCustomers.ToListAsync();

        public async Task<NormalCustomer> GetByUniqueIdentifierAsync(string phone) =>
            await _context.NormalCustomers.FirstOrDefaultAsync(c => c.PhoneNumber == phone);

        public async Task AddAsync(NormalCustomer entity)
        {
            await _context.NormalCustomers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NormalCustomer entity)
        {
            _context.NormalCustomers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.NormalCustomers.FindAsync(id);
            if (entity != null)
            {
                _context.NormalCustomers.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }

}

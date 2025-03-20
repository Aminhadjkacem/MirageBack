using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.Input;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories.Impl
{

    public class NormalCustomerRepository : INormalCustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public NormalCustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NormalCustomer>> GetAllAsync() =>
            await _context.NormalCustomers.ToListAsync();

        public async Task<NormalCustomer> GetByPhoneNumberAsync(string phone) =>
            await _context.NormalCustomers.FirstOrDefaultAsync(c => c.PhoneNumber == phone);

        public async Task<NormalCustomer> AddAsync(NormalCustomer entity)
        {
            await _context.NormalCustomers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<NormalCustomer> UpdateAsync(string phone, NormalCustomer dto)
        {
            NormalCustomer existingCustomer = await _context.NormalCustomers.FirstOrDefaultAsync(c => c.PhoneNumber == phone);
            if (existingCustomer == null)
                return null;

            // Update only the provided fields
            if (!string.IsNullOrWhiteSpace(dto.FullName))
                existingCustomer.FullName = dto.FullName;

            if (!string.IsNullOrWhiteSpace(dto.Address))
                existingCustomer.Address = dto.Address;

            if (!string.IsNullOrWhiteSpace(dto.Email))
                existingCustomer.Email = dto.Email;

            if (!string.IsNullOrWhiteSpace(dto.CIN))
                existingCustomer.CIN = dto.CIN;

            _context.NormalCustomers.Update(existingCustomer);
            await _context.SaveChangesAsync();

            return existingCustomer;
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

        public async Task<NormalCustomer> GetByIdAsync(int id)
        {
            return await _context.NormalCustomers
                .FirstOrDefaultAsync(nc => nc.Id == id);
        }
    }

}

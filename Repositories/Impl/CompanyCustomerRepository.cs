using DHP_Manager.Data;
using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace DHP_Manager.Repositories.Impl
{
    public class CompanyCustomerRepository : ICompanyCustomerRepository // Correctly specify the type here
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

        public async Task<CompanyCustomer> GetByMatriculeFiscalAsync(string matriculeFiscal)
        {
            return await _context.CompanyCustomers
                                 .FirstOrDefaultAsync(c => c.MatriculeFiscal == matriculeFiscal);
        }
        public async Task<CompanyCustomer> AddAsync(CompanyCustomer customer)
        {
            await _context.CompanyCustomers.AddAsync(customer); // Add the customer to the context
            await _context.SaveChangesAsync(); // Save changes to the database

            // Return the saved customer (with its generated ID and other properties if necessary)
            return customer;
        }


        public async Task<CompanyCustomer> UpdateAsync(CompanyCustomer customer)
        {
            _context.CompanyCustomers.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteAsync(string matriculeFiscal)
        {
            var customer = await GetByMatriculeFiscalAsync(matriculeFiscal);
            if (customer != null)
            {
                _context.CompanyCustomers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<CompanyCustomer> GetByIdAsync(int id)
        {
            return await _context.CompanyCustomers
                .FirstOrDefaultAsync(cc => cc.Id == id);
        }
    }
}

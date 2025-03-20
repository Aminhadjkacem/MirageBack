using DHP_Manager.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DHP_Manager.Repositories.Interface
{
    public interface ICompanyCustomerRepository
    {
        Task<IEnumerable<CompanyCustomer>> GetAllAsync();
        Task<CompanyCustomer> GetByMatriculeFiscalAsync(string matriculeFiscal);
        Task<CompanyCustomer> AddAsync(CompanyCustomer customer);
        Task<CompanyCustomer> UpdateAsync(CompanyCustomer customer);
        Task DeleteAsync(string matriculeFiscal);
        Task<CompanyCustomer> GetByIdAsync(int id);
    }
}

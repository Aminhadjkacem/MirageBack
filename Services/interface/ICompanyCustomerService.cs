using System.Collections.Generic;
using System.Threading.Tasks;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Details;

namespace DHP_Manager.Services.Interface
{
    public interface ICompanyCustomerService
    {
        Task<IEnumerable<CompanyCustomerDetails>> GetAllAsync();
        Task<CompanyCustomerDetails> GetByMatriculeFiscalAsync(string matriculeFiscal);
        Task<CompanyCustomerDetails> AddAsync(CompanyCustomerInput customer);
        Task<CompanyCustomerDetails> UpdateAsync(string matriculeFiscal, CompanyCustomerInput customer);
        Task<bool> DeleteAsync(string matriculeFiscal);
        Task<CompanyCustomer> GetByIdAsync(int id);
    }
}

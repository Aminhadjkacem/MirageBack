using System.Collections.Generic;
using System.Threading.Tasks;
using DHP_Manager.Models.Entities;
using DHP_Manager.Repositories.Interface;
using DHP_Manager.Services.Interface;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Details;

namespace DHP_Manager.Services.impl
{
    public class CompanyCustomerService : ICompanyCustomerService
    {
        private readonly ICompanyCustomerRepository _repository;

        public CompanyCustomerService(ICompanyCustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CompanyCustomerDetails>> GetAllAsync()
        {
            IEnumerable<CompanyCustomer> customers = await _repository.GetAllAsync();

            // Apply fromEntity to each item in the IEnumerable
            return customers.Select(customer => CompanyCustomerDetails.fromEntity(customer));
        }


        public async Task<CompanyCustomerDetails> GetByMatriculeFiscalAsync(string matriculeFiscal)
        {
            CompanyCustomer companyCustomer = await _repository.GetByMatriculeFiscalAsync(matriculeFiscal);
            return CompanyCustomerDetails.fromEntity(companyCustomer);
        }

        public async Task<CompanyCustomerDetails> AddAsync(CompanyCustomerInput customer)
        {
            CompanyCustomer Customer=await _repository.AddAsync(customer.ToEntity());
            CompanyCustomerDetails companyCustomerDetails = CompanyCustomerDetails.fromEntity(Customer);
            return companyCustomerDetails;
        }

        public async Task<CompanyCustomerDetails> UpdateAsync(string matriculeFiscal, CompanyCustomerInput customer)
        {
            CompanyCustomer existingCustomer = await _repository.GetByMatriculeFiscalAsync(matriculeFiscal);
            if (existingCustomer == null) return null;

            existingCustomer.CompanyName = customer.CompanyName;
            existingCustomer.Address = customer.Address;
            existingCustomer.Ville = customer.Ville;
            existingCustomer.Email = customer.Email;
            existingCustomer.MatriculeFiscal = customer.MatriculeFiscal;
            existingCustomer.Contact1Name = customer.Contact1Name;
            existingCustomer.Contact1Phone = customer.Contact1Phone;
            existingCustomer.Contact2Name = customer.Contact2Name;
            existingCustomer.Contact2Phone = customer.Contact2Phone;
            existingCustomer.Contact3Name = customer.Contact3Name;
            existingCustomer.Contact3Phone = customer.Contact3Phone;

            CompanyCustomer Customer = await _repository.UpdateAsync(existingCustomer);
            CompanyCustomerDetails companyCustomerDetails = CompanyCustomerDetails.fromEntity(Customer);
            return companyCustomerDetails;
        }

        public async Task<bool> DeleteAsync(string matriculeFiscal)
        {
            var existingCustomer = await _repository.GetByMatriculeFiscalAsync(matriculeFiscal);
            if (existingCustomer == null) return false;

            await _repository.DeleteAsync(matriculeFiscal);
            return true;
        }
    }
}

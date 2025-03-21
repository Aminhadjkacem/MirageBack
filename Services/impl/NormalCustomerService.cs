using DHP_Manager.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DHP_Manager.Services.Interface;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.Details;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.Input;
using DHP_Manager.Repositories.Impl;
namespace DHP_Manager.Services.impl
{
    public class NormalCustomerService:INormalCustomerService
    {
        private readonly INormalCustomerRepository _repository;

        public NormalCustomerService(INormalCustomerRepository repository)
        {
            _repository = repository;
        }

        //Service must not return DTO
        public async Task<IEnumerable<NormalCustomerDetails>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            return customers.Select(c => new NormalCustomerDetails
            {
                id = c.Id,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                Email = c.Email,
                CIN = c.CIN
            });
        }

        public async Task<NormalCustomerDetails> GetByPhoneAsync(string phone)
        {
            NormalCustomer customer = await _repository.GetByPhoneNumberAsync(phone);
            if (customer == null) return null;

            return NormalCustomerDetails.fromEntity(customer);
        }

        public async Task<NormalCustomerDetails> AddAsync(NormalCustomerInput dto)
        {
            NormalCustomer customer = await _repository.AddAsync(dto.ToEntity());
            NormalCustomerDetails customerDetails = NormalCustomerDetails.fromEntity(customer);
            return customerDetails;
        }

        public async Task<NormalCustomerDetails> UpdateAsync(string phone, NormalCustomerInput dto)
        {
            NormalCustomer existingCustomer = await _repository.GetByPhoneNumberAsync(phone);
            if (existingCustomer == null) return null;
            Console.WriteLine("Existing Customer: " + existingCustomer.FullName);
            existingCustomer.FullName = dto.FullName;
            existingCustomer.Address = dto.Address;
            existingCustomer.Email = dto.Email;
            existingCustomer.PhoneNumber = dto.PhoneNumber;
            existingCustomer.CIN = dto.CIN;

            NormalCustomer customer = await _repository.UpdateAsync(phone ,existingCustomer);
            NormalCustomerDetails customerDetails = NormalCustomerDetails.fromEntity(customer);

            return (customerDetails);
        }

        public async Task<bool> DeleteAsync(string phone)
        {
            var existingCustomer = await _repository.GetByPhoneNumberAsync(phone);
            if (existingCustomer == null) return false;

            await _repository.DeleteAsync(existingCustomer.Id);
            return true;
        }

        public async Task<NormalCustomer> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
    }
}

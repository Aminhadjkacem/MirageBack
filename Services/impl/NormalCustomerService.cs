using DHP_Manager.Entities;
using DHP_Manager.Repositories.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DHP_Manager.Services.Interface;
using DHP_Manager.Models.DTOs;
namespace DHP_Manager.Services.impl
{
    public class NormalCustomerService:INormalCustomerService
    {
        private readonly ICustomerRepository<NormalCustomer> _repository;

        public NormalCustomerService(ICustomerRepository<NormalCustomer> repository)
        {
            _repository = repository;
        }

        //Service must not return DTO
        public async Task<IEnumerable<NormalCustomerDTO>> GetAllAsync()
        {
            var customers = await _repository.GetAllAsync();
            return customers.Select(c => new NormalCustomerDTO
            {
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                Email = c.Email,
                CIN = c.CIN
            });
        }

        public async Task<NormalCustomerDTO> GetByPhoneAsync(string phone)
        {
            var customer = await _repository.GetByUniqueIdentifierAsync(phone);
            if (customer == null) return null;

            return new NormalCustomerDTO
            {
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                Email = customer.Email,
                CIN = customer.CIN
            };
        }

        public async Task<NormalCustomerDTO> AddAsync(NormalCustomerDTO dto)
        {
            var customer = new NormalCustomer
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Email = dto.Email,
                CIN = dto.CIN
            };

            await _repository.AddAsync(customer);
            return dto;
        }

        public async Task<NormalCustomerDTO> UpdateAsync(string phone, NormalCustomerDTO dto)
        {
            var existingCustomer = await _repository.GetByUniqueIdentifierAsync(phone);
            if (existingCustomer == null) return null;

            existingCustomer.FullName = dto.FullName;
            existingCustomer.Address = dto.Address;
            existingCustomer.Email = dto.Email;
            existingCustomer.CIN = dto.CIN;

            await _repository.UpdateAsync(existingCustomer);
            return dto;
        }

        public async Task<bool> DeleteAsync(string phone)
        {
            var existingCustomer = await _repository.GetByUniqueIdentifierAsync(phone);
            if (existingCustomer == null) return false;

            await _repository.DeleteAsync(existingCustomer.Id);
            return true;
        }
    }
}

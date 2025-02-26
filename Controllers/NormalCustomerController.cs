using DHP_Manager.Models.Details;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.response;
using DHP_Manager.Services.impl;
using DHP_Manager.Services.Interface;
using Microsoft.AspNetCore.Mvc;



namespace DHP_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NormalCustomerController : ControllerBase
    {
        private readonly INormalCustomerService _service;

        public NormalCustomerController(INormalCustomerService service)
        {
            _service = service;
        }

        // Get all customers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        // Get customer by phone number
        [HttpGet("{phone}")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            NormalCustomerDetails customer = await _service.GetByPhoneAsync(phone);
            if (customer == null)
                return NotFound($"No customer found with phone number: {phone}");

            return Ok(NormalCustomerResponse.fromDetails(customer));
        }

        // Create a new customer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NormalCustomerDto customerDto)
        {
            if (customerDto == null)
                return null;

            NormalCustomerDetails createdCustomer = await _service.AddAsync(customerDto.ToInput());
            return Ok(NormalCustomerResponse.fromDetails(createdCustomer));
        }

        // Update customer details
        [HttpPatch("{phone}")]
        public async Task<IActionResult> Update(string phone, [FromBody] NormalCustomerDto customerDto)
        {
            if (customerDto == null)
                return BadRequest("Customer data is required.");

            NormalCustomerDetails updatedCustomer = await _service.UpdateAsync(phone, customerDto.ToInput());
            if (updatedCustomer == null)
                return NotFound($"No customer found with phone number: {phone}");

            return Ok(updatedCustomer);
        }


        // Delete customer by phone number
        [HttpDelete("{phone}")]
        public async Task<IActionResult> Delete(string phone)
        {
            var deleted = await _service.DeleteAsync(phone);
            if (!deleted)
                return NotFound($"No customer found with phone number: {phone}");

            return NoContent();
        }
    }
}

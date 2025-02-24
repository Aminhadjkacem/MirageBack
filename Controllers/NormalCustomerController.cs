using DHP_Manager.Entities;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Services.impl;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DHP_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NormalCustomerController : ControllerBase
    {
        private readonly NormalCustomerService _service;

        public NormalCustomerController(NormalCustomerService service)
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
            var customer = await _service.GetByPhoneAsync(phone);
            if (customer == null)
                return NotFound($"No customer found with phone number: {phone}");

            return Ok(customer);
        }

        // Create a new customer
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NormalCustomerDTO customerDto)
        {
            if (customerDto == null)
                return BadRequest("Customer data is required.");

            var createdCustomer = await _service.AddAsync(customerDto);
            return CreatedAtAction(nameof(GetByPhone), new { phone = createdCustomer.PhoneNumber }, createdCustomer);
        }

        // Update customer details
        [HttpPut("{phone}")]
        public async Task<IActionResult> Update(string phone, [FromBody] NormalCustomerDTO customerDto)
        {
            if (customerDto == null)
                return BadRequest("Customer data is required.");

            var updatedCustomer = await _service.UpdateAsync(phone, customerDto);
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

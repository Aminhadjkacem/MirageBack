using DHP_Manager.Models.response;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using DHP_Manager.Models.Details;
namespace DHP_Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyCustomerController : ControllerBase
    {
        private readonly ICompanyCustomerService _service;

        public CompanyCustomerController(ICompanyCustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyCustomerDetails>>> GetAll()
        {
            var customers = await _service.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{matriculeFiscal}")]
        public async Task<ActionResult<CompanyCustomerResponse>> GetByMatriculeFiscal(string matriculeFiscal)
        {
            var customer = await _service.GetByMatriculeFiscalAsync(matriculeFiscal);
            if (customer == null) return NotFound("Customer not found.");
            return Ok(customer);
        }

        [HttpPost]
        public async Task<ActionResult<CompanyCustomerResponse>> Create(CompanyCustomerDtO customer)
        {
            CompanyCustomerDetails createdCustomer = await _service.AddAsync(CompanyCustomerDtO.toInput(customer));
            return CreatedAtAction(nameof(GetByMatriculeFiscal), new { matriculeFiscal = createdCustomer.MatriculeFiscal }, createdCustomer);
        }

        [HttpPatch("{matriculeFiscal}")]
        public async Task<ActionResult<CompanyCustomerResponse>> Update(string matriculeFiscal, CompanyCustomerDtO customer)
        {
            CompanyCustomerDetails updatedCustomer = await _service.UpdateAsync(matriculeFiscal, CompanyCustomerDtO.toInput(customer));
            if (updatedCustomer == null) return NotFound("Customer not found.");
            return Ok(updatedCustomer);
        }

        [HttpDelete("{matriculeFiscal}")]
        public async Task<ActionResult> Delete(string matriculeFiscal)
        {
            var deleted = await _service.DeleteAsync(matriculeFiscal);
            if (!deleted) return NotFound("Customer not found.");
            return NoContent();
        }
    }
}

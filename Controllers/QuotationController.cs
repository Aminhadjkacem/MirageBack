using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.input;
using DHP_Manager.Models.Input;
using DHP_Manager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DHP_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotationController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        // GET: api/quotations
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var quotations = await _quotationService.GetAllQuotationsAsync();
            return Ok(quotations);  // Return the list of QuotationDetails
        }

        // GET: api/quotations/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var quotation = await _quotationService.GetQuotationByIdAsync(id);
            if (quotation == null) return NotFound();

            return Ok(quotation);  // Return the QuotationDetails
        }

        // POST: api/quotations
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] QuotationDto quotationDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var newQuotation = await _quotationService.CreateQuotationAsync(QuotationDto.ToInput(quotationDto));
            return CreatedAtAction(nameof(GetById), new { id = newQuotation.Id }, newQuotation);  // Return QuotationDetails
        }

        // PUT: api/quotations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] QuotationDto quotationDto)
        {
            var updatedQuotation = await _quotationService.UpdateQuotationAsync(id, QuotationDto.ToInput(quotationDto));
            return Ok(updatedQuotation);  // Return QuotationDetails
        }

        // DELETE: api/quotations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _quotationService.DeleteQuotationAsync(id);
            if (!success) return NotFound();

            return NoContent();  // No content to return, just confirmation
        }
    }
}

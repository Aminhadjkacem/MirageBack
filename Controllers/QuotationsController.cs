using Microsoft.AspNetCore.Mvc;
using DHP_Manager.Models.Input;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.response;
using DHP_Manager.Services.Interface;
using DHP_Manager.Models.input;
using Microsoft.EntityFrameworkCore;
using DHP_Manager.Helpers.Exceptions;

namespace DHP_Manager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationsController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotationsController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<QuotationResponse>>> GetAllQuotations()
        {
            var quotations = await _quotationService.GetAllQuotationsAsync();
            return Ok(quotations);
        }

        [HttpGet("validated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<QuotationResponse>>> GetValidatedQuotations()
        {
            var validatedQuotations = await _quotationService.GetValidatedQuotationsAsync();
            return Ok(validatedQuotations);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<QuotationResponse>> GetQuotation(int id)
        {
            try
            {
                var quotation = await _quotationService.GetQuotationWithDesignationsAsync(id);
                return Ok(quotation);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/validate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ValidateQuotation(int id)
        {
            try
            {
                await _quotationService.ValidateQuotationAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<QuotationResponse>> CreateQuotation([FromBody] QuotationInput quotationInput)
        {
            try
            {
                if (quotationInput == null)
                    return BadRequest("Quotation input cannot be null.");

                if (string.IsNullOrWhiteSpace(quotationInput.ProjectName))
                    return BadRequest("Project name cannot be empty");

                var createdQuotation = await _quotationService.CreateQuotationAsync(quotationInput);
                return CreatedAtAction(nameof(GetQuotation), new { id = createdQuotation.Id }, createdQuotation);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InsufficientStockException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateQuotation([FromBody] QuotationUpdateInput quotationInput)
        {
            if (quotationInput.Id <= 0 || quotationInput.Id != quotationInput.Id)
                return BadRequest("Invalid or mismatched quotation ID");

            try
            {
                if (string.IsNullOrWhiteSpace(quotationInput.ProjectName))
                    return BadRequest("Project name cannot be empty");

                await _quotationService.UpdateQuotationAsync(quotationInput);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest($"Database update failed: {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteQuotation(int id)
        {
            try
            {
                await _quotationService.DeleteQuotationAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
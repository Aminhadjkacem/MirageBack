using Microsoft.AspNetCore.Mvc;
using DHP_Manager.Models.Input;
using DHP_Manager.Models.Entities;
using DHP_Manager.Models.response;
using DHP_Manager.Services.Interface;

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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<QuotationResponse>> CreateQuotation([FromBody] QuotationInput quotationInput)
        {
            try
            {
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
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateQuotation(int id, [FromBody] Quotation quotation)
        {
            if (id != quotation.Id)
                return BadRequest("Quotation ID mismatch");

            try
            {
                if (string.IsNullOrWhiteSpace(quotation.ProjectName))
                    return BadRequest("Project name cannot be empty");

                await _quotationService.UpdateQuotationAsync(quotation);
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
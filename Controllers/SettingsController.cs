using DHP_Manager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DHP_Manager.Services.Interface;

namespace DHP_Manager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [HttpGet("all-settings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllSettings()
        {
            try
            {
                var settings = new
                {
                    ProfitMargin = await _settingsService.GetProfitMarginAsync() * 100,
                    TVAMargin = await _settingsService.GetTVAMarginAsync() * 100,
                    DroitDuTimbrePrice = await _settingsService.GetDroitDuTimbrePriceAsync(),
                    CompanyTel = await _settingsService.GetCompanyTelAsync(),
                    CompanyFiscal = await _settingsService.GetCompanyFiscalAsync(),
                    FactureId = await _settingsService.GetFactureIdAsync()
                };

                return Ok(settings);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("profit-margin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfitMargin([FromBody] decimal newMargin)
        {
            try
            {
                await _settingsService.UpdateProfitMarginAsync(newMargin / 100);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("tva-margin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTVAMargin([FromBody] decimal newMargin)
        {
            try
            {
                await _settingsService.UpdateTVAMarginAsync(newMargin / 100);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("droit-du-timbre")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateDroitDuTimbrePrice([FromBody] decimal newPrice)
        {
            try
            {
                await _settingsService.UpdateDroitDuTimbrePriceAsync(newPrice);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("company-tel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompanyTel([FromBody] string companyTel)
        {
            try
            {
                await _settingsService.UpdateCompanyTelAsync(companyTel);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("company-fiscal")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCompanyFiscal([FromBody] string companyFiscal)
        {
            try
            {
                await _settingsService.UpdateCompanyFiscalAsync(companyFiscal);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}

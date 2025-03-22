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

        [HttpPut("profit-margin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfitMargin([FromBody] decimal newMargin)
        {
            try
            {
                await _settingsService.UpdateProfitMarginAsync(newMargin);
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
                await _settingsService.UpdateTVAMarginAsync(newMargin);
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
    }
}
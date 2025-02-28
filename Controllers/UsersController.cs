using DHP_Manager.Models.Details;
using DHP_Manager.Models.DTOs;
using DHP_Manager.Models.response;
using DHP_Manager.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DHP_Manager.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticationDTO authenticationDTO)
        {
            UserDetails? response = await _userService.Authenticate(authenticationDTO);
            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(AuthenticateResponse.fromDetails(response));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO createUserDTO)
        {
            try
            {
                UserDetails userDetails = await _userService.CreateUserAsync(CreateUserDTO.toInput(createUserDTO));
                // Optionally, you can auto-authenticate and return a token.
                return Ok(AuthenticateResponse.fromDetails(userDetails));
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

using BackEnd.DTOs.Auth;
using BackEnd.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Superpower.Parsers;

namespace BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] BackEnd.DTOs.Auth.LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result.Token == "")
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] BackEnd.DTOs.Auth.RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result.Token == "")
                return BadRequest(result);

            return Ok(result);
        }
    }
}

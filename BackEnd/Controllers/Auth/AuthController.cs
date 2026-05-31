using BackEnd.DTOs.Auth;
using BackEnd.Services.Interfaces.Auth;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Superpower.Parsers;
using BackEnd.ErrorHandling;

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

            if (!result.Success)
                return BadRequest(result);

            return Ok(result.Data);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(BackEnd.DTOs.Auth.RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result.Data);
        }

    }
}

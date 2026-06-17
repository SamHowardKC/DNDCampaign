using BackEnd.DTOs.Auth;
using BackEnd.ErrorHandling;
using BackEnd.Services.Auth.Interface;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Superpower.Model;
using Superpower.Parsers;

namespace BackEnd.Controllers.Auth
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

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,      
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(1),
                Path = "/"
            };

            return Ok(new
            {
                error = result.Error,
                userID = result.Data.UserID,
                username = result.Data.Username,
                token = result.Data.Token,
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(BackEnd.DTOs.Auth.RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,   
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(1),
                Path = "/"
            };

            return Ok(new
            {
                error = result.Error,
                token = result.Data.Token,
                userID = result.Data.UserID,
                username = result.Data.Username
            });
        }

    }
}

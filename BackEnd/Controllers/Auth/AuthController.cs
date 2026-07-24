using BackEnd.DTOs.Auth;
using BackEnd.Services.Auth.Interface;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return BadRequest(result);

            // The client authenticates with an Authorization: Bearer header, so the
            // token is returned in the response body for the client to store and send.
            return Ok(new
            {
                userID = result.Data.UserID,
                username = result.Data.Username,
                token = result.Data.Token,
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return Ok(new
            {
                token = result.Data.Token,
                userID = result.Data.UserID,
                username = result.Data.Username
            });
        }

    }
}

using BackEnd.DTOs.Auth;

namespace BackEnd.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    }
}

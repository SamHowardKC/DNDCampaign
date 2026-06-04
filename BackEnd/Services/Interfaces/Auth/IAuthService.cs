using BackEnd.DTOs.Auth;
using BackEnd.ErrorHandling;

namespace BackEnd.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
        Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);

    }
}

using BackEnd.DTOs.Auth;
using BackEnd.ErrorHandling;

namespace BackEnd.Services.Auth.Interface
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request);
        Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request);

    }
}

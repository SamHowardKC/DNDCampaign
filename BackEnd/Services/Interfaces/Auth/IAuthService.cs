using BackEnd.DTOs.Auth;
using BackEnd.ErrorHandling;

namespace BackEnd.Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
        Task<Result<RegisterResponse>> RegisterAsync(RegisterRequest request);

    }
}

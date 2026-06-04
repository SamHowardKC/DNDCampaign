using BackEnd.Entities.Auth;

namespace BackEnd.Services.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}

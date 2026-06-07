using BackEnd.Entities.Auth;

namespace BackEnd.Services.Auth.Interface
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}

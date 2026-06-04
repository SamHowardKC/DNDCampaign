using BackEnd.Entities.Auth;

namespace BackEnd.Services.Interfaces.Auth
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task CreateAsync(User user);
    }
}

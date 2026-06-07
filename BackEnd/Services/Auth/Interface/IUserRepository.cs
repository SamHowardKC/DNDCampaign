using BackEnd.Entities.Auth;

namespace BackEnd.Services.Auth.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task CreateAsync(User user);
    }
}

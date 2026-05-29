using BackEnd.Data;
using BackEnd.Services.Interfaces.Auth;
using BackEnd.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Implementation.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _Context;

        public UserRepository(AppDbContext context)
        {
            _Context = context;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _Context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(User user)
        {
            _Context.Users.Add(user);
            await _Context.SaveChangesAsync();
        }
    }
}

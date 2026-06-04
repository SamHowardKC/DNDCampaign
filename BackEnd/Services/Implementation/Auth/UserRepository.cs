using BackEnd.Data;
using BackEnd.Entities.Auth;
using Microsoft.EntityFrameworkCore;
using BackEnd.Services.Interfaces.Auth;

namespace BackEnd.Services.Implementation.Auth
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task CreateAsync(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}

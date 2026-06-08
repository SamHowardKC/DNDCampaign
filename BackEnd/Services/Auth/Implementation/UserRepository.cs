using BackEnd.Data;
using BackEnd.Entities.Auth;
using BackEnd.Entities.Campaign;
using BackEnd.Services.Auth.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Auth.Implementation
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

        public async Task<User> CreateAsync(User user)
        {
            await _context.User.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}

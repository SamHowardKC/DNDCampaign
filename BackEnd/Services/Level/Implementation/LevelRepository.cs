using BackEnd.Data;
using BackEnd.Entities.Auth;
using BackEnd.Entities.Level;
using BackEnd.Services.Level.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Level.Implementation
{
    public class LevelRepository
    {
        private readonly AppDbContext _context;
        public LevelRepository(AppDbContext context)
        {
            _context = context;
        }

        
        public async Task<BackEnd.Entities.Level.Level?> GetLevelByXpAsync(int Xp)
        {
            return await _context.Level
                .Where(l => l.XPToReach >= Xp)
                .OrderBy(l => l.XPToReach)
                .FirstOrDefaultAsync();
        }

        
    }
}

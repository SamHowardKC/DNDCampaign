using BackEnd.Data;
using BackEnd.Entities.Character;
using BackEnd.Services.Character.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Character.Implementation
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly AppDbContext _context;
        public CharacterRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Entities.Character.Character>> GetByUserAsync(Guid userId)
        {
            return await _context.Character.Where(c => c.Id == userId).ToListAsync();
        }
    }
}

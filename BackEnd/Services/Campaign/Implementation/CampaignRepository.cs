using BackEnd.Data;
using BackEnd.Entities.Campaign;
using BackEnd.Services.Campaign.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.Campaign.Implementation
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly AppDbContext _context;
        public CampaignRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Entities.Campaign.Campaign>> GetByUserAsync(Guid userId)
        {
            return await _context.Campaign
                .Where(c => c.DungeonMasterID == userId)
                .ToListAsync();
        }
    }
}

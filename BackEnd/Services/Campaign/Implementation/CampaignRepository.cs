using BackEnd.Data;
using BackEnd.Entities.Auth;
using BackEnd.Entities.Campaign;
using BackEnd.Entities.Character;
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

        public async Task<List<Entities.Campaign.Campaign>> GetByDMAsync(Guid userId)
        {
            return await _context.Campaign
                .Where(c => c.DungeonMasterID == userId)
                .ToListAsync();
        }
        public async Task<List<Entities.Campaign.Campaign>> GetByCharactersAsync(List<Entities.Character.Character> characters)
        {
            var characterIds = characters.Select(c => c.Id).ToList();

            return await _context.CharacterCampaign
                .Where(cc => characterIds.Contains(cc.CharacterID))
                .Select(cc => cc.Campaign)   // return Campaign entity
                .Distinct()
                .ToListAsync();
        }

        public async Task<Entities.Campaign.Campaign> AddAsync(Entities.Campaign.Campaign campaign)
        {
            await _context.Campaign.AddAsync(campaign);
            await _context.SaveChangesAsync();
            return campaign;
        }
    }
}

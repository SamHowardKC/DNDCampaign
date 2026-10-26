using BackEnd.Data;
using BackEnd.Services.CharacterCampaign.Interface;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services.CharacterCampaign.Implementation
{
    public class CharacterCampaignRepository : ICharacterCampaignRepository
    {
        private readonly AppDbContext _context;
        public CharacterCampaignRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Entities.Character.CharacterCampaign>> GetByCampaignAsync(Guid campaignId)
        {
            return await _context.CharacterCampaign.Where(c => c.CampaignID == campaignId).ToListAsync(); ;
        }
    }
}

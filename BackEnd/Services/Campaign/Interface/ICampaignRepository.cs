using BackEnd.Entities.Character;

namespace BackEnd.Services.Campaign.Interface
{
    public interface ICampaignRepository
    {
        Task<List<Entities.Campaign.Campaign>> GetByDMAsync(Guid userId); // all campaigns where the user is the DM
        Task<List<Entities.Campaign.Campaign>> GetByCharactersAsync(List<Entities.Character.Character> characters); // all campaigns where the user is a player
    }
}

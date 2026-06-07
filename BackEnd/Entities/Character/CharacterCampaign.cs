using Microsoft.EntityFrameworkCore;
using BackEnd.Entities.Campaign;

namespace BackEnd.Entities.Character
{
    public class CharacterCampaign
    {
        public Guid CampaignID { get; set; }
        public BackEnd.Entities.Campaign.Campaign Campaign { get; set; } = default!;

        public Guid CharacterID { get; set; }
        public Character Character { get; set; } = default!;

        public int CharacterXP { get; set; } 
        public int CharacterHP { get; set; }
        public int CharacterMaxHP { get; set; } 
    }
}

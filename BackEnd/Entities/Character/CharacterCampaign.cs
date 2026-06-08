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

        public int Xp { get; set; } 
        public int Hp { get; set; }
        public int MaxHp { get; set; } 
        public DateTimeOffset CreatedAt { get; set; }
    }
}

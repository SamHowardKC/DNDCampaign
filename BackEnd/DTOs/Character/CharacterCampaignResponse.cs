using BackEnd.Entities;

namespace BackEnd.DTOs.Character
{
    public class CharacterCampaignResponse
    {
        public List<CharacterCampaignListItem> Campaigns { get; set; } = new List<CharacterCampaignListItem>();
    }

    public class CharacterCampaignListItem : BaseEntity
    {
        public int Xp { get; set; } = default!;
        public int MaxHp { get; set; } = default!;
        public int Hp { get; set; } = default!;
    }
}
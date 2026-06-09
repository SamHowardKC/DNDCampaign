using BackEnd.Entities.Character;

namespace BackEnd.Entities.Campaign
{
    public class Campaign : BaseEntity
    {
        public string Name { get; set; } = default!;
        public ICollection<CharacterCampaign> CharacterCampaigns { get; set; } = new List<CharacterCampaign>();
        public Guid DungeonMasterID { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public bool IsEnded { get; set; } = false;
    }
}

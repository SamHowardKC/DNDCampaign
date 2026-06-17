using BackEnd.Entities;

namespace BackEnd.DTOs.Campaign
{
    public class CampaignListResponse
    {
        public List<CampaignListItem> Campaigns { get; set; } = new List<CampaignListItem>();
    }

    public class CampaignListItem : BaseEntity
    {
        public required string Name { get; set; }
        public Guid DungeonMasterID { get; set; }
        public required string DungeonMasterName { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnded { get; set; }
        public bool IsDungeonMaster { get; set; }
    }
}

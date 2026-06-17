using BackEnd.Entities;

namespace BackEnd.DTOs.Campaign
{
    public class ActiveCampaignListResponse
    {
        public List<ActiveCampaignListItem> Campaigns { get; set; } = new List<ActiveCampaignListItem>();
    }

    public class ActiveCampaignListItem : BaseEntity
    {
        public required string Name { get; set; }
        public Guid DungeonMasterID { get; set; }
        public required string DungeonMasterName { get; set; }
        public bool IsDungeonMaster { get; set; }
    }
}

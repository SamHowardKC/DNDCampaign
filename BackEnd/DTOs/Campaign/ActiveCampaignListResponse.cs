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
        public required int NumberOfPlayers { get; set; }
        public required decimal AveragePlayerLevel { get; set; }
        public required int PlayerLevel { get; set; }
    }
}

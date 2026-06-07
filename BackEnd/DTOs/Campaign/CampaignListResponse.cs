namespace BackEnd.DTOs.Campaign
{
    public class CampaignListResponse
    {
        public List<CampaignListItem> Campaigns { get; set; } = new List<CampaignListItem>();
    }

    public class CampaignListItem
    {
        public Guid CampaignID { get; set; }
        public required string CampaignName { get; set; }
        public Guid DungeonMasterID { get; set; }
        public bool IsActive { get; set; }
        public bool IsEnded { get; set; }
        public bool IsDungeonMaster { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}

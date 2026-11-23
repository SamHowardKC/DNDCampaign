namespace BackEnd.DTOs.Campaign
{
    public class ActiveCampaignListResponse
    {
        public List<ActiveCampaignListItem> Campaigns { get; set; } = new List<ActiveCampaignListItem>();
    }

    public class ActiveCampaignListItem
    {
        public required Guid Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required string Name { get; set; }
        public required Guid DungeonMasterID { get; set; }
        public required string DungeonMasterName { get; set; }
        public required bool IsDungeonMaster { get; set; }
        public required int NumberOfPlayers { get; set; }
        public required decimal AveragePlayerLevel { get; set; }
        public required int PlayerLevel { get; set; }
    }
}

namespace BackEnd.Entities.Campaign
{
    public class Campaign
    {
        public Guid CampaignID { get; set; } = Guid.NewGuid();
        public string CampaignName { get; set; } = default!;
        public Guid DungeonMasterID { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public bool IsEnded { get; set; } = false;
        //public bool IsDungeonMaster { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}

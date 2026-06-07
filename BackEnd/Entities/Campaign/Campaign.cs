namespace BackEnd.Entities.Campaign
{
    public class Campaign : BaseEntity
    {
        public string CampaignName { get; set; } = default!;
        public Guid DungeonMasterID { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public bool IsEnded { get; set; } = false;
    }
}

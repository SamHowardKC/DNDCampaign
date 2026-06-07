namespace BackEnd.Entities.Character
{
    public class CharacterCampaign
    {
        public Guid CampaignID { get; set; } = default!;
        public Guid CharacterID { get; set; } = default!;
        public Guid CharacterXP { get; set; } = default!;
        public Guid CharacterHP { get; set; } = default!;
        public Guid CharacterMaxHP { get; set; } = default!;
    }
}

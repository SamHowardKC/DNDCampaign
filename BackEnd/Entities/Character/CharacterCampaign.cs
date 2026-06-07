namespace BackEnd.Entities.Character
{
    public class CharacterCampaign
    {
        public Guid CampaignID { get; set; } = default!;
        public Campaign.Campaign Campaign { get; set; } = default!;
        public Guid CharacterID { get; set; } = default!;
        public Character Character { get; set; } = default!;
        public int CharacterXP { get; set; } = default!;
        public int CharacterHP { get; set; } = default!;
        public int CharacterMaxHP { get; set; } = default!;
    }
}

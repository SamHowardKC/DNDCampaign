namespace BackEnd.DTOs.Character
{
    public class CharacterCampaignListItem
    {
        public required Guid CampaignID { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required int Xp { get; set; }
        public required int MaxHp { get; set; }
        public required int Hp { get; set; }
        // Not required until Level is mapped in CharacterService
        public int Level { get; set; }
    }
}
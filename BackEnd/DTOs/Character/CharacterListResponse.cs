namespace BackEnd.DTOs.Character
{
    public class CharacterListResponse
    {
        public List<CharacterListItem> Characters { get; set; } = new List<CharacterListItem>();
    }

    public class CharacterListItem
    {
        public required Guid Id { get; set; }
        public required DateTimeOffset CreatedAt { get; set; }
        public required string Name { get; set; }
        public required Guid ClassID { get; set; }
        public List<CharacterCampaignListItem> Campaigns { get; set; } = new List<CharacterCampaignListItem>();
    }
}

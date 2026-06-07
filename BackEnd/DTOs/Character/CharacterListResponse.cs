using BackEnd.Entities;

namespace BackEnd.DTOs.Character
{
    public class CharacterListResponse
    {
        public List<CharacterListItem> Campaigns { get; set; } = new List<CharacterListItem>();
    }

    public class CharacterListItem : BaseEntity
    {
        public required string CharacterName { get; set; }
        public Guid ClassID { get; set; }
    }
}

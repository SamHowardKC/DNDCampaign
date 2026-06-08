using BackEnd.Entities;
using BackEnd.Entities.Character;
using BackEnd.DTOs.Character;

namespace BackEnd.DTOs.Character
{
    public class CharacterListResponse
    {
        public List<CharacterListItem> Characters { get; set; } = new List<CharacterListItem>();
    }

    public class CharacterListItem : BaseEntity
    {
        public required string Name { get; set; }
        public Guid ClassID { get; set; }
        public CharacterCampaignResponse Campaigns {  get; set; } = new CharacterCampaignResponse();
    }
}

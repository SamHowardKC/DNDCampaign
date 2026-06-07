namespace BackEnd.Entities.Character
{
    public class Character : CharacterBase
    {
        public ICollection<CharacterCampaign> CharacterCampaigns { get; set; } = new List<CharacterCampaign>();
        public Guid UserID { get; set; } = default!;
        public Guid ClassID { get; set; } = default!;
    }
}

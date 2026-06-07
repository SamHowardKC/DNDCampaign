namespace BackEnd.Entities.Character
{
    public class Character
    {
        public Guid CharacterID { get; set; } = Guid.NewGuid();
        public Guid UserID { get; set; } = default!;
        public Guid ClassID { get; set; } = default!;
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public string CharacterName { get; set; } = default!;
    }
}

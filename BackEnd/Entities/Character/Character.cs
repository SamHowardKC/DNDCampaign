namespace BackEnd.Entities.Character
{
    public class Character : CharacterBase
    {
        public Guid UserID { get; set; } = default!;
        public Guid ClassID { get; set; } = default!;
    }
}

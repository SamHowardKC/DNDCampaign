namespace BackEnd.Entities.Character
{
    public class CharacterClass 
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ClassName { get; set; } = default!;
        public int MaxHPIncrease { get; set; } = default!;
    }
}

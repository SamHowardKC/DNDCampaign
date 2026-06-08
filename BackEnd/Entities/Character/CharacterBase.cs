// include a base class for when I want to have NPCs as well

namespace BackEnd.Entities.Character
{
    public abstract class CharacterBase : BaseEntity
    {
        public string Name { get; set; } = default!;
    }
}

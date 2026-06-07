namespace BackEnd.Services.Character.Interface
{
    public interface ICharacterService
    {
        public Task<List<Entities.Character.Character>> GetCharactersForUserAsync(Guid userId);
    }
}

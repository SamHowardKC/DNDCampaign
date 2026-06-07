namespace BackEnd.Services.Character.Interface
{
    public interface ICharacterRepository
    {
        Task<List<Entities.Character.Character>> GetByUserAsync(Guid userId);
    }
}

using BackEnd.DTOs.Character;
using BackEnd.ErrorHandling;

namespace BackEnd.Services.Character.Interface
{
    public interface ICharacterService
    {
        public Task<Result<CharacterListResponse>> GetCharactersForUserAsync(Guid userId);
    }
}

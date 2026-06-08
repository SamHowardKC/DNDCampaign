using BackEnd.DTOs.Campaign;
using BackEnd.DTOs.Character;
using BackEnd.Entities.Auth;
using BackEnd.Entities.Character;
using BackEnd.ErrorHandling;
using BackEnd.Services.Character.Interface;

namespace BackEnd.Services.Character.Implementation
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;
        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }
        public async Task<Result<CharacterListResponse>> GetCharactersForUserAsync(Guid userId)
        {
            var charactersEntityList = await _characterRepository.GetByUserAsync(userId);
            if (charactersEntityList == null)
            {
                return Result<CharacterListResponse>.Fail("No characters found for the user.");
            }

            var response = new CharacterListResponse
            {
                Characters = charactersEntityList.Select(c => new CharacterListItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    ClassID = c.ClassID,
                    CreatedAt = c.CreatedAt,

                    Campaigns = new CharacterCampaignResponse
                    {
                        Campaigns = c.CharacterCampaigns.Select(cc => new CharacterCampaignListItem
                        {
                            Xp = cc.Xp,
                            MaxHp = cc.MaxHp,
                            Hp = cc.Hp,
                            CreatedAt = cc.CreatedAt
                        }).ToList()
                    }
                }).ToList()
            };

            return Result<CharacterListResponse>.Ok(response);
        }
    }
}

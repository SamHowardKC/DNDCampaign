using BackEnd.DTOs.Campaign;
using BackEnd.Entities.Campaign;
using BackEnd.ErrorHandling;
using BackEnd.Services.Campaign.Interface;
using BackEnd.Services.Character.Interface;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using System.Runtime.InteropServices;

namespace BackEnd.Services.Campaign.Implementation
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly ICharacterRepository _characterRepository;
        public CampaignService(ICampaignRepository campaignRepository,
            ICharacterRepository characterRepository)
        {
            _campaignRepository = campaignRepository;
            _characterRepository = characterRepository;
        }

        public async Task<Result<CampaignListResponse>> GetCampaignsForUserAsync(Guid userID)
        {
            // this is a work in progress
            var DMCampaignList = await _campaignRepository.GetByDMAsync(userID);

            var CharacterList = await _characterRepository.GetByUserAsync(userID);

            var PlayerCampaignList = await _campaignRepository.GetByCharactersAsync(CharacterList);

            var CampaignList = DMCampaignList.Union(PlayerCampaignList).ToList();

            if (DMCampaignList == null)
            {
                return Result<CampaignListResponse>.Fail("No campaigns found for the user.");
            }

            var response = new CampaignListResponse
            {
                Campaigns = CampaignList.Select(c => new CampaignListItem
                {
                    CampaignID = c.Id,
                    CampaignName = c.CampaignName,
                    DungeonMasterID = c.DungeonMasterID,
                    IsActive = c.IsActive,
                    IsEnded = c.IsEnded,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };

            return Result<CampaignListResponse>.Ok(response);
        }
    }
}

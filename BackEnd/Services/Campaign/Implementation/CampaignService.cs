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
                    Id = c.Id,
                    CampaignName = c.CampaignName,
                    DungeonMasterID = c.DungeonMasterID,
                    IsActive = c.IsActive,
                    IsEnded = c.IsEnded,
                    CreatedAt = c.CreatedAt,
                    IsDungeonMaster = c.DungeonMasterID == userID
                }).ToList()
            };

            return Result<CampaignListResponse>.Ok(response);
        }

        public async Task<Result<CampaignListItem>> CreateCampaignAsync(CreateCampaignRequest request, Guid userID)
        {
            // to do: add validation here to check if campaign with that name already exists, if it does then name it CampaignName-2 or CampaignName-3 etc.

            var newCampaign = new BackEnd.Entities.Campaign.Campaign
            {
                Id = Guid.NewGuid(),
                CampaignName = request.CampaignName,
                DungeonMasterID = userID,
                IsActive = true,
                IsEnded = false,
                CreatedAt = DateTime.UtcNow
            };
            var createdCampaign = await _campaignRepository.AddAsync(newCampaign);
            if (createdCampaign == null)
            {
                return Result<CampaignListItem>.Fail("Failed to create campaign.");
            }
            var response = new CampaignListItem
            {
                Id = createdCampaign.Id,
                CampaignName = createdCampaign.CampaignName,
                DungeonMasterID = createdCampaign.DungeonMasterID,
                IsActive = createdCampaign.IsActive,
                IsEnded = createdCampaign.IsEnded,
                CreatedAt = createdCampaign.CreatedAt,
                IsDungeonMaster = true
            };
            return Result<CampaignListItem>.Ok(response);
        }
    }
}

using BackEnd.DTOs.Campaign;
using BackEnd.Entities.Campaign;
using BackEnd.Entities.Auth;
using BackEnd.ErrorHandling;
using BackEnd.Services.Campaign.Interface;
using BackEnd.Services.Character.Interface;
using BackEnd.Services.Auth;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;
using System.Runtime.InteropServices;
using BackEnd.Services.Auth.Interface;

namespace BackEnd.Services.Campaign.Implementation
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;
        private readonly ICharacterRepository _characterRepository;
        private readonly IUserRepository _userRepository;
        public CampaignService(ICampaignRepository campaignRepository,
            ICharacterRepository characterRepository,
            IUserRepository userRepository)
        {
            _campaignRepository = campaignRepository;
            _characterRepository = characterRepository;
            _userRepository = userRepository;
        }

        public async Task<Result<ActiveCampaignListResponse>> GetActiveCampaignsForUserAsync(Guid userID)
        {
            var user = await _userRepository.GetByIdAsync(userID);
            if (user == null)
                return Result<ActiveCampaignListResponse>.Fail("No user found.");

            // campaigns where user is dm
            var dmCampaignList = await _campaignRepository.GetByDMAsync(userID);
            if (dmCampaignList == null)
                return Result<ActiveCampaignListResponse>.Fail("No campaigns found for the user.");

            // all characters of user
            var characterList = await _characterRepository.GetByUserAsync(userID);
            
            // all campaigns where the user has a character
            var playerCampaignList = await _campaignRepository.GetByCharactersAsync(characterList);

            // list of active campaigns where user is either a player or dm 
            var campaignList = dmCampaignList
                .Union(playerCampaignList)
                .Distinct()
                .Where(c => c.IsActive && !c.IsEnded)
                .ToList();

            var response = new ActiveCampaignListResponse
            {
                Campaigns = campaignList.Select(c => new ActiveCampaignListItem
                {
                    Id = c.Id,
                    Name = c.Name,
                    DungeonMasterID = c.DungeonMasterID,
                    DungeonMasterName = user.Username,
                    CreatedAt = c.CreatedAt,
                    IsDungeonMaster = c.DungeonMasterID == userID
                }).ToList()
            };

            return Result<ActiveCampaignListResponse>.Ok(response);
        }

        public async Task<Result<ActiveCampaignListItem>> CreateCampaignAsync(CreateCampaignRequest request, Guid userID)
        {
            int count;

            var user = await _userRepository.GetByIdAsync(userID);
            if (user == null)
                return Result<ActiveCampaignListItem>.Fail("No user found.");

            var existingCampaign = await _campaignRepository.GetByDMAsync(userID);

            if (existingCampaign != null)
            {
                count = existingCampaign.Count;
                request.Name = 
                    request.Name 
                    + "-" 
                    + (count + 1).ToString();
            }

            var newCampaign = new BackEnd.Entities.Campaign.Campaign
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                DungeonMasterID = userID,
                IsActive = true,
                IsEnded = false,
                CreatedAt = DateTime.UtcNow
            };
            var createdCampaign = await _campaignRepository.AddAsync(newCampaign);
            if (createdCampaign == null)
            {
                return Result<ActiveCampaignListItem>.Fail("Failed to create campaign.");
            }
            var response = new ActiveCampaignListItem
            {
                Id = createdCampaign.Id,
                Name = createdCampaign.Name,
                DungeonMasterName = user.Username,
                DungeonMasterID = createdCampaign.DungeonMasterID,
                CreatedAt = createdCampaign.CreatedAt,
                IsDungeonMaster = true
            };
            return Result<ActiveCampaignListItem>.Ok(response);
        }
    }
}

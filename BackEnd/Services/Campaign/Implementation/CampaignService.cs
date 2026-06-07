using BackEnd.DTOs.Campaign;
using BackEnd.Entities.Campaign;
using BackEnd.ErrorHandling;
using BackEnd.Services.Campaign.Interface;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq;

namespace BackEnd.Services.Campaign.Implementation
{
    public class CampaignService : ICampaignService
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignService(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        public async Task<Result<CampaignListResponse>> GetCampaignsForUserAsync(Guid userID)
        {
            // this is a work in progress
            var DMCampaignList = await _campaignRepository.GetByUserAsync(userID);

            if (DMCampaignList == null)
            {
                return Result<CampaignListResponse>.Fail("No campaigns found for the user.");
            }

            /*var response = new CampaignListResponse
            {
                Campaigns = CampaignList.Select(c => new CampaignListItem
                {
                    CampaignID = c.CampaignID,
                    CampaignName = c.CampaignName,
                    DungeonMasterID = c.DungeonMasterID,
                    IsActive = c.IsActive,
                    IsEnded = c.IsEnded,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };*/

            return Result<CampaignListResponse>.Ok(response);
        }
    }
}
